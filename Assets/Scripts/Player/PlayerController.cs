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
        UpdateDirection();
        UpdateMovement();
        Attack();
    }
    void UpdateDirection()
    {
        Direction = Vector3.zero;
        if (Input.GetKey(KeyCode.A)) Direction += Vector3.left;
        if (Input.GetKey(KeyCode.D)) Direction += Vector3.right;
        if (Input.GetKey(KeyCode.W)) Direction += Vector3.up;
        if (Input.GetKey(KeyCode.S)) Direction += Vector3.down;

        //if (Direction != Vector3.zero) transform.rotation = Quaternion.LookRotation(Vector3.forward, -Direction);
        animator.SetFloat(GlobalConstants.HorizontalVelocity, Direction.x);
        animator.SetFloat(GlobalConstants.VerticalVelocity, Direction.y);
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
                //sword.Attack();
                animator.SetTrigger(GlobalConstants.SlashTrigger);
                AttackCoolDownMarker = Time.time;
            }
        }
    }
    public void TakeDamage(int dmg)
    {

        Debug.Log("dmg");
        CurrentHealth -= dmg;
    }
    public void Heal(int heal)
    {

        Debug.Log("heal");
        CurrentHealth += heal;
    }
}
