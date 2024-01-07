using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
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
            if(HealthBarController.Instance != null) HealthBarController.Instance.UpdateHealthBar((float)currentHealth / MaxHealth);
        }
    }
    Vector3 Direction = Vector2.zero;
    public float Speed = 2f;
    Rigidbody2D rigidbody2D;
    Animator animator;
    public float AttackCooldown = 0.5f;
    float AttackCoolDownMarker = 0f;
    public Sword sword;
    bool FreezePlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        CurrentHealth = MaxHealth;
        MapGen.Instance.GenerateTiles((int)PlayerController.Instance.transform.position.x, (int)PlayerController.Instance.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
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
        rigidbody2D.velocity = Vector3.zero;
    }
    void UpdateDirection()
    {
        Direction = Vector3.zero;
        if (Input.GetKey(KeyCode.A)) Direction += Vector3.left;
        if (Input.GetKey(KeyCode.D)) Direction += Vector3.right;
        if (Input.GetKey(KeyCode.W)) Direction += Vector3.up;
        if (Input.GetKey(KeyCode.S)) Direction += Vector3.down;

        //if (Direction != Vector3.zero) transform.rotation = Quaternion.LookRotation(Vector3.forward, -Direction);
        if (Direction == Vector3.zero)
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
        rigidbody2D.velocity = Speed * Direction.normalized;
    }
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if ( Time.time > AttackCoolDownMarker + AttackCooldown)
            {
                if(animator.GetFloat(GlobalConstants.VerticalVelocity) == -1) { animator.Play(GlobalConstants.HumanAttackDown); }
                else if(animator.GetFloat(GlobalConstants.HorizontalVelocity) == -1) { animator.Play(GlobalConstants.HumanAttackLeft); }
                else if(animator.GetFloat(GlobalConstants.HorizontalVelocity) == 1) { animator.Play(GlobalConstants.HumanAttackRight); }
                else if (animator.GetFloat(GlobalConstants.VerticalVelocity) == 1) { animator.Play(GlobalConstants.HumanAttackUp); }
                else { animator.Play(GlobalConstants.HumanAttackDown); }
                sword.Attack();
                AttackCoolDownMarker = Time.time;
                StartCoroutine(AttackDelay(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
            }
        }
    }
    IEnumerator AttackDelay(float delay)
    {
        FreezePlayer = true;
        yield return new WaitForSeconds(delay);
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
}
