using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;
using TMPro;

public class PlayerController : NetworkBehaviour
{
    public static PlayerController Instance;

    public SpawnParameter[] SpawnParameters;

    public GameObject PlayerUI;
    public HealthBarController HealthBarController;
    public TextMeshProUGUI DistanceText;

    [SerializeField]
    int MaxHealth = 100;
    [Networked]
    public int currentHealth { get; set; }
    public int CurrentHealth
    {
        get { return currentHealth; }
        set {
            currentHealth = value;
            if (currentHealth < 0) currentHealth = 0;
            if (currentHealth > MaxHealth) currentHealth = MaxHealth;

            if (HasInputAuthority)
            {
                HealthBarController.UpdateHealthBar((float)currentHealth / MaxHealth);
            }
        }
    }
    [SerializeField]
    float MaxSpeed = 2.5f;
    Rigidbody2D rigidbody2D;
    Animator animator;
    public float AttackCooldown = 0.5f;
    float AttackCoolDownMarker = 0f;
    public Sword sword;
    bool FreezePlayer = false;
    NetworkInputData data;

    [Networked]
    bool Slash
    {
        get;
        set;
    }
    [Networked]
    Vector2 Direction
    {
        get;
        set;
    }
    [Networked]
    Vector3 Position
    {
        get;
        set;
    }
    [Networked]
    bool IsDead { get; set; }

    protected ChangeDetector _changeDetector;
    public override void Spawned()
    {
        if(HasInputAuthority)
        {
            Instance = this;
            Position = transform.position;
        }
        transform.position = Position;
        animator = GetComponentInChildren<Animator>(); 
        rigidbody2D = GetComponent<Rigidbody2D>();
        if (HasStateAuthority) CurrentHealth = MaxHealth;
        else CurrentHealth = currentHealth;
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
        PropertiesChanged();
    }

    protected virtual void PropertiesChanged()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(currentHealth):
                    CurrentHealth = currentHealth;
                    break;
                case nameof(IsDead):
                    Death();
                    break;
            }
        }
    }
    void Start()
    {
        PlayerUI.SetActive(HasInputAuthority);

        if (HasInputAuthority)
        {
            MapGen.Instance.GenerateTiles((int)PlayerController.Instance.transform.position.x, (int)PlayerController.Instance.transform.position.y);
            SpawnManager.Instance.AddPlayer(this);
        }

        sword.SetOwner(this);
    }
    void Freeze()
    {
        rigidbody2D.velocity = Vector3.zero;
        Direction = Vector2.zero;
    }
    void UpdateDirection()
    {
        //if (Direction != Vector3.zero) transform.rotation = Quaternion.LookRotation(Vector3.forward, -Direction);
        if (Direction == Vector2.zero)
        {
            animator.SetBool(GlobalConstants.Idle, true);
        }
        else
        {
            animator.SetBool(GlobalConstants.Idle, false);
            animator.SetFloat(GlobalConstants.HorizontalVelocity, Direction.x);
            animator.SetFloat(GlobalConstants.VerticalVelocity, Direction.y);
        }
    }
    void UpdateMovement()
    {
        rigidbody2D.velocity = MaxSpeed * Direction.normalized;
        //_cc.Move(Speed * data.direction);
    }
    void Attack()
    {
        if (Slash)
        {
            if ( Time.time > AttackCoolDownMarker + AttackCooldown)
            {
                RPC_SwordAttack();
                AttackCoolDownMarker = Time.time;
                FreezePlayer = true;
            }
        }
    }
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_SwordAttack()
    {
        if (Mathf.Abs(animator.GetFloat(GlobalConstants.VerticalVelocity)) < Mathf.Abs(animator.GetFloat(GlobalConstants.HorizontalVelocity)))
        {
            if (Mathf.Sign(animator.GetFloat(GlobalConstants.HorizontalVelocity)) == 1) animator.Play(GlobalConstants.HumanAttackRight);
            else animator.Play(GlobalConstants.HumanAttackLeft);
        }
        else
        {
            if (Mathf.Sign(animator.GetFloat(GlobalConstants.VerticalVelocity)) == 1) animator.Play(GlobalConstants.HumanAttackUp);
            else animator.Play(GlobalConstants.HumanAttackDown);
        }
        sword.Attack();
    }
    public void Unfreeze()
    {
        FreezePlayer = false;
    }
    public void TakeDamage(int dmg)
    {
        if (!HasInputAuthority) return;
        CurrentHealth -= dmg;
    }
    public void Heal(int heal)
    {
        CurrentHealth += heal;
    }
    public bool IsFullHealth()
    {
        return currentHealth == MaxHealth;
    }
    private void OnDestroy()
    {
        //SpawnManager.Instance.RemovePlayer(this);
    }
    public override void FixedUpdateNetwork()
    {
        PropertiesChanged();
    }
    public void FixedUpdate()
    {
        if (IsDead) return;
        if (HasInputAuthority)
        {
            DistanceText.text = transform.position.magnitude.ToString("F0") + " m";

            Direction = Vector3.zero;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                Direction += Vector2.up;

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                Direction += Vector2.down;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                Direction += Vector2.left;

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                Direction += Vector2.right;

            if (MobileInputController.Instance != null)
            {
                if (MobileInputController.Instance.Direction().magnitude != 0)
                {
                    Direction = MobileInputController.Instance.Direction();
                }
                Slash = Input.GetKey(KeyCode.Space) || MobileInputController.Instance.SlashPressed;
            }
        }
        
        if (FreezePlayer)
        {
            Freeze();
            return;
        }
        UpdateDirection();
        UpdateMovement();
        Attack();
        if (HasInputAuthority)
        {
            Position = transform.position;
        }
        else{
            if ((Position - transform.position).magnitude >= 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, Position, Time.fixedDeltaTime);
            }
        }
    }
    protected virtual void Death()
    {
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        IsDead = true;
        animator.SetTrigger(GlobalConstants.DeadTrigger);
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
}