using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtoController : AIController
{
    public float AttackRange = 2f;
    public int Damage = 10;
    bool Freeze = false;
    bool Unrooted = false;
    public GameObject DirtProjectilePrefab;

    public float BulletSpeed;
    public Transform ProjectileSpawnLocation;
    protected override void UpdateDirection()
    {
        Direction = Vector3.zero;

        if (Freeze)
        {
            rigidbody2D.velocity = UnityEngine.Random.Range(0f, Speed) * Direction.normalized;
            return;
        }
        if (PlayerDistance() > AggroRange)
        {
            if (Unrooted)
            {
                animator.SetFloat(GlobalConstants.HorizontalVelocity, 0);
                Unrooted = false;
                animator.SetTrigger(GlobalConstants.DigDown);
            }
        }
        else if (PlayerDistance() < AttackRange && ReadyToAttack() && Unrooted)
        {
            animator.SetTrigger(GlobalConstants.Attack);
            AttackCoolDownMarker = Time.time;
            Freeze = true;

            return;
        }
        else
        {
            if (!Unrooted)
            {
                animator.SetTrigger(GlobalConstants.DigUp);
                Unrooted = true;
                Freeze = true;
            }
                
            if (ReadyToAttack())
            {
                Direction = BasicFollowDirection();
            }
            else
            {
                Direction = BasicFleeDirection();
            }
        }
        
    }
    protected override void UpdateAnimation()
    {
        if (Math.Sign(Direction.x) == 0) return;
        animator.SetFloat(GlobalConstants.HorizontalVelocity, Math.Sign(Direction.x));
    }
    protected override void Death()
    {
        if (projectile != null && projectile.GetComponent<Rigidbody2D>().velocity.magnitude == 0) Destroy(projectile.gameObject);
        rigidbody2D.velocity = Vector2.zero;
        IsDead = true;
        animator.Play(GlobalConstants.DeadTrigger);
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
    bool ReadyToAttack()
    {
        return (Time.time > AttackCoolDownMarker + AttackCooldown);
    }
    float PlayerDistance()
    {
        PlayerController target = SpawnManager.Instance.players[0];
        foreach (PlayerController player in SpawnManager.Instance.players)
        {
            if ((player.transform.position - transform.position).magnitude < (target.transform.position - transform.position).magnitude)
            {
                target = player;
            }
        }
        return (target.transform.position - transform.position).magnitude;
    }
    public void Unfreeze()
    {
        Freeze = false;
    }
    Projectile projectile;
    void SpawnProjectile()
    {
        projectile = Instantiate(DirtProjectilePrefab, ProjectileSpawnLocation.position, Quaternion.identity).GetComponent<Projectile>();
    }
    public void ShootProjectile()
    {
        projectile.Shoot(Damage, BasicFollowDirection(), BulletSpeed);
    }
}
