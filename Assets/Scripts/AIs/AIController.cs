using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : NetworkBehaviour
{
    public int MaxHealth = 100;
    [Networked] public int CurrentHealth { get; set; }
    public float MovementUpdateTime = 1f;
    protected float MovementUpdateTimeMarker;
    public float Speed = 2f;
    protected Rigidbody2D rigidbody2D;
    protected Animator animator;
    [Networked] public bool IsDead { get; set; } = false;
    public float AggroRange = 4f;

    public float AttackCooldown = 1f;
    protected float AttackCoolDownMarker = 0f;

    public List<GameObject> DropablePrefabs;

    protected ChangeDetector _changeDetector;

    public SpawnParameter spawnParameter;


    [Networked]
    protected Vector3 Direction
    {
        get;
        set;
    }
    [Networked]
    protected Vector3 Position
    {
        get;
        set;
    }

    // Start is called before the first frame update
    public override void Spawned()
    {
        CurrentHealth = MaxHealth;
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        MovementUpdateTimeMarker = Time.time;
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }
    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;
        if (IsDead) return;

        Position = transform.position;

        UpdateDirection();
        UpdateAnimation();
        UpdateMovement();
    }
    private void FixedUpdate()
    {
        PropertiesChanged();

        if (HasStateAuthority) return;
        if (IsDead) return;

        UpdateAnimation();
        UpdateMovement();

        if ((Position - transform.position).magnitude != 0)
        {
            transform.position = Vector3.Lerp(Position, transform.position, Time.fixedDeltaTime * 2f / ((Position - transform.position).magnitude));
        }
    }
    protected virtual void PropertiesChanged()
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
            }
        }
    }
    protected virtual void UpdateDirection()
    {
        //animator.SetFloat(GlobalConstants.HorizontalVelocity, Math.Sign(Direction.x));
    }
    protected virtual void UpdateAnimation()
    {
        animator.SetFloat(GlobalConstants.HorizontalVelocity, Math.Sign(Direction.x));
    }
    protected virtual void UpdateMovement()
    {
        rigidbody2D.velocity = Speed * Direction.normalized;
        //if (Direction != Vector3.zero) transform.rotation = Quaternion.LookRotation(Vector3.forward, -Direction);
    }
    protected virtual void BasicFollow()
    {
        PlayerController target = SpawnManager.Instance.players[0];
        foreach (PlayerController player in SpawnManager.Instance.players)
        {
            if ((player.transform.position - transform.position).magnitude < (target.transform.position - transform.position).magnitude)
            {
                target = player;
            }
        }
        if ((target.transform.position - transform.position).magnitude < AggroRange)
        {
            Direction = (target.transform.position - transform.position).normalized;
        }
    }
    protected virtual Vector3 BasicFollowDirection()
    {
        PlayerController target = SpawnManager.Instance.players[0];
        foreach (PlayerController player in SpawnManager.Instance.players)
        {
            if ((player.transform.position - transform.position).magnitude < (target.transform.position - transform.position).magnitude)
            {
                target = player;
            }
        }
        
        return (target.transform.position - transform.position).normalized;
    }
    protected virtual Vector3 BasicFleeDirection()
    {
        PlayerController target = SpawnManager.Instance.players[0];
        foreach (PlayerController player in SpawnManager.Instance.players)
        {
            if ((player.transform.position - transform.position).magnitude < (target.transform.position - transform.position).magnitude)
            {
                target = player;
            }
        }

        return -(target.transform.position - transform.position).normalized;
    }
    protected void RandomWalk()
    {
        Direction = new Vector3(UnityEngine.Random.Range(-1f, 2f), UnityEngine.Random.Range(-1f, 2f), 0);
    }
    protected virtual void Attack()
    {
    }
    protected virtual void Death()
    {
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        IsDead = true;
        animator.SetTrigger(GlobalConstants.DeadTrigger);
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
    public void SpawnDropables()
    {
        if (!HasStateAuthority) return;
        
        foreach (GameObject dropable in DropablePrefabs)
        {
            Runner.Spawn(dropable, transform.position, Quaternion.identity);
        }
    }
    public virtual void TakeDmg(int dmg)
    {
        if (!HasStateAuthority) return;

        if (IsDead) return;

        CurrentHealth -= dmg;
        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Death();
        }
    }
    public void DestroyThyself()
    {
        spawnParameter.CurrentAmount--;
        Runner.Despawn(Object);
    }
}
