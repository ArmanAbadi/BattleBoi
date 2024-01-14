using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManController : AIController
{

    public float AttackRange = 2f;
    public int Damage = 10;
    bool TreeAwoken = false;
    public GameObject ProjectilePrefab;

    public float BulletSpeed = 5f;

    List<Projectile> projectile = new List<Projectile>();

    public GameObject TreeSpawnpointTop;
    public GameObject TreeSpawnpointLeft;
    public GameObject TreeSpawnpointRight;
    protected override IEnumerator MovementUpdate()
    {
        while (!IsDead)
        {
            Direction = Vector3.zero;

            if (PlayerDistance() > AggroRange)
            {
                animator.SetBool(GlobalConstants.Aggro, false); 
                TreeAwoken = false;
            }
            else if (PlayerDistance() < AttackRange && ReadyToAttack() && TreeAwoken)
            {
                animator.SetTrigger(GlobalConstants.Attack);
                AttackCoolDownMarker = Time.time;

                yield return null;
                continue;
            }
            else
            {
                animator.SetBool(GlobalConstants.Aggro, true);
                TreeAwoken = true;
                Direction = BasicFollowDirection();
            }
            if (Direction.x > 0f)
            {
                animator.SetFloat(GlobalConstants.HorizontalVelocity, 1);
            }
            else if (Direction.x < 0f)
            {
                animator.SetFloat(GlobalConstants.HorizontalVelocity, -1);
            }
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
        for(int i = projectile.Count-1; i >= 0; i--)
        {
            if (projectile != null && projectile[i].GetComponent<Rigidbody2D>().velocity.magnitude == 0) Destroy(projectile[i].gameObject);
        }
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
    void SpawnProjectile()
    {
        //projectile.Add(Instantiate(DirtProjectilePrefab, ProjectileSpawnLocation.position, Quaternion.identity).GetComponent<Projectile>());
        //Projectile tempProjectile = Instantiate(DirtProjectilePrefab, ProjectileSpawnLocation.position, Quaternion.identity).GetComponent<Projectile>();
        //tempProjectile.Shoot(Damage, BasicFollowDirection(), BulletSpeed);
        //projectile.Add(tempProjectile);
    }
}
