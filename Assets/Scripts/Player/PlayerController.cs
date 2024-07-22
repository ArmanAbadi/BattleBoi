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
    int currentHealth;
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

    public override void Spawned()
    {
        if(NetworkManager.Instance._runner.LocalPlayer == GetComponent<NetworkObject>().InputAuthority)
        {
            Instance = this;
        }
        animator = GetComponentInChildren<Animator>(); 
        rigidbody2D = GetComponent<Rigidbody2D>();
        CurrentHealth = MaxHealth;
    }

    void Start()
    {
        MapGen.Instance.GenerateTiles((int)PlayerController.Instance.transform.position.x, (int)PlayerController.Instance.transform.position.y);
        PlayerUI.SetActive(HasInputAuthority);
        SpawnManager.Instance.AddPlayer(this);
    }
    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;

        if (!GetInput(out data)) return;

        Slash = data.SlashAttack;
        data.direction.Normalize();
        Direction = data.direction;
        Position = transform.position;

        if (FreezePlayer)
        {
            Freeze();
            return;
        }
        UpdateDirection();
        UpdateMovement();
        Attack();
    }
    void Freeze()
    {
        //rigidbody2D.velocity = Vector3.zero;
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
                if(Mathf.Abs(animator.GetFloat(GlobalConstants.VerticalVelocity)) < Mathf.Abs(animator.GetFloat(GlobalConstants.HorizontalVelocity))){
                    if(Mathf.Sign(animator.GetFloat(GlobalConstants.HorizontalVelocity)) == 1) animator.Play(GlobalConstants.HumanAttackRight);
                    else animator.Play(GlobalConstants.HumanAttackLeft);
                }
                else
                {
                    if (Mathf.Sign(animator.GetFloat(GlobalConstants.VerticalVelocity)) == 1) animator.Play(GlobalConstants.HumanAttackUp);
                    else animator.Play(GlobalConstants.HumanAttackDown);
                }

                sword.Attack();
                AttackCoolDownMarker = Time.time;
                FreezePlayer = true;
            }
        }
    }
    public void Unfreeze()
    {
        FreezePlayer = false;
    }
    public void TakeDamage(int dmg)
    {
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
        SpawnManager.Instance.RemovePlayer(this);
    }

    public void FixedUpdate()
    {
        if (HasInputAuthority)
        {
            DistanceText.text = transform.position.magnitude.ToString("F0")+ " m";
        }
        if (HasStateAuthority) return;

        if (FreezePlayer)
        {
            Freeze();
            return;
        }
        UpdateDirection();
        UpdateMovement();
        Attack();

        if ((Position - transform.position).magnitude != 0)
        {
            transform.position = Vector3.Lerp(Position, transform.position, Time.fixedDeltaTime * 2f / ((Position - transform.position).magnitude));
        }
    }
}