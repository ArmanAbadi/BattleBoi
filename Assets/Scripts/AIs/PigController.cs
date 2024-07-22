using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigController : AIController
{
    public float AggroAlliesRange = 10f;
    [Networked] bool aggrod { get; set; } = false;
    protected bool Aggrod
    {
        get {
            return aggrod; 
        }
        set {
            aggrod = value;
        }
    }
    public float AggroSpeed = 5f;
    public int Damage = 10;
    protected void Start()
    {
        MonsterManager.AddPig(this);
    }
    public override void Spawned()
    {
        base.Spawned();
        animator.SetBool(GlobalConstants.Aggro, aggrod);
    }
    protected override void UpdateDirection()
    {
        if (Aggrod)
        {
            BasicFollow();
        }
        else
        {
            if (Time.time - MovementUpdateTimeMarker > MovementUpdateTime)
            {
                MovementUpdateTimeMarker = Time.time;
                RandomWalk();
            }
        }
    }
    protected override void UpdateMovement()
    {
        rigidbody2D.velocity = Random.Range(0f, Aggrod? AggroSpeed:Speed) * Direction.normalized;
    }
    protected override void PropertiesChanged()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(CurrentHealth):
                    if (CurrentHealth <= 0)
                    {
                        CurrentHealth = 0;
                        Death();
                    }
                    break;
                case nameof(aggrod):
                    animator.SetBool(GlobalConstants.Aggro, aggrod);
                    break;
            }
        }
    }
    public override void TakeDmg(int dmg)
    {
        if (!HasStateAuthority) return;
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
