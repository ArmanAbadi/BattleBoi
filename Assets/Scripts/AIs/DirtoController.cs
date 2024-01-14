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
    protected override IEnumerator MovementUpdate()
    {
        while (!IsDead)
        {
            Direction = Vector3.zero;

            if (Freeze)
            {
                rigidbody2D.velocity = Random.Range(0f, Speed) * Direction.normalized;
                yield return null;
                continue;
            }

            if (PlayerDistance() > AggroRange)
            {
                if (Unrooted)
                {
                    Unrooted = false;
                    animator.SetTrigger(GlobalConstants.DigDown);
                }
            }
            else if (PlayerDistance() < AttackRange && ReadyToAttack() && Unrooted)
            {
                animator.SetTrigger(GlobalConstants.Attack);
                AttackCoolDownMarker = Time.time;
                Freeze = true;

                yield return null;
                continue;
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
            if (Direction.x > 0f)
            {
                animator.SetFloat(GlobalConstants.HorizontalVelocity, 1);
            }
            else if(Direction.x < 0f)
            {
                animator.SetFloat(GlobalConstants.HorizontalVelocity, -1);
            }
            rigidbody2D.velocity = Random.Range(0f, Speed) * Direction.normalized;
            yield return null;
        }
    }
    public override void TakeDmg(int dmg)
    {
        if (IsDead) return;
        CurrentHealth -= dmg;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Death();
        }
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
        return (PlayerController.Instance.transform.position - transform.position).magnitude;
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
