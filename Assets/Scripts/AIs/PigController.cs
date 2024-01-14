using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigController : AIController
{
    public float AggroAlliesRange = 10f;
    bool aggrod = false;
    protected bool Aggrod
    {
        get { return aggrod; }
        set {
            aggrod = value;
            animator.SetBool(GlobalConstants.Aggro, aggrod);
        }
    }
    public float AggroSpeed = 5f;
    public int Damage = 10;
    protected void Start()
    {
        base.Start();
        MonsterManager.AddPig(this);
    }
    protected override IEnumerator MovementUpdate()
    {
        while (!IsDead)
        {
            UpdateDirection();
            UpdateMovement();

            yield return new WaitForSeconds(Aggrod? 0:MovementUpdateTime);
        }
    }
    protected override void UpdateDirection()
    {
        Direction = Vector3.zero;
        if (Aggrod)
        {
            BasicFollow();
        }
        else
        {
            RandomWalk();
        }
        
        animator.SetFloat(GlobalConstants.HorizontalVelocity, Direction.x);
    }
    protected override void UpdateMovement()
    {
        rigidbody2D.velocity = Random.Range(0f, Aggrod? AggroSpeed:Speed) * Direction.normalized;
    }
    public override void TakeDmg(int dmg)
    {
        if (IsDead) return;
        Aggrod = true;
        AggroAllies();
        CurrentHealth -= dmg;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Death();
        }
    }
    void AggroAllies()
    {
        foreach(PigController pig in MonsterManager.Pigs)
        {
            if ((transform.position - pig.transform.position).magnitude < AggroAlliesRange)
            {
                pig.Aggrod = true;
            }
        }
    }
    protected override void BasicFollow()
    {
        Direction += (PlayerController.Instance.transform.position - transform.position).normalized;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Attack(collision);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Attack(collision);
    }
    void Attack(Collision2D collision)
    {
        if (Aggrod && collision.gameObject.CompareTag(GlobalConstants.Tags.Player.ToString()))
        {
            if (Time.time > AttackCoolDownMarker + AttackCooldown)
            {

                Debug.Log("pig atk");
                PlayerController.Instance.TakeDamage(Damage);
                AttackCoolDownMarker = Time.time;
            }
        }
    }
    private void OnDestroy()
    {
        MonsterManager.RemovePig(this);
    }
}
