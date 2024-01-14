using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManController : AIController
{

    public float AttackRange = 2f;
    public int Damage = 10;
    bool TreeAwoken = false;
    public GameObject ProjectilePrefab;

    public float BulletSpeed = 3f;

    List<Projectile> Projectiles = new List<Projectile>();

    public GameObject TreeSpawnpointTop;
    public GameObject TreeSpawnpointLeft;
    public GameObject TreeSpawnpointRight;

    public float ShootDelayTime = 1f;
    protected override IEnumerator MovementUpdate()
    {
        while (!IsDead)
        {
            Direction = Vector3.zero;

            if (PlayerDistance() > AggroRange)
            {
                animator.SetBool(GlobalConstants.Aggro, false); 
            }
            else if (PlayerDistance() < AttackRange && ReadyToAttack() && TreeAwoken)
            {
                AttackCoolDownMarker = Time.time;
                SpawnProjectile();
                yield return null;
                continue;
            }
            else
            {
                animator.SetBool(GlobalConstants.Aggro, true);
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
        for(int i = Projectiles.Count-1; i >= 0; i--)
        {
            if (Projectiles != null && Projectiles[i].GetComponent<Rigidbody2D>().velocity.magnitude == 0) Destroy(Projectiles[i].gameObject);
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
        Projectile tempProjectile = Instantiate(ProjectilePrefab, TreeSpawnpointTop.transform.position, TreeSpawnpointTop.transform.rotation, TreeSpawnpointTop.transform).GetComponent<Projectile>();
        tempProjectile.GetComponent<SpriteRenderer>().sortingOrder = 3;
        Projectiles.Add(tempProjectile);
        StartCoroutine(ShootWithDelay(tempProjectile));

        tempProjectile = Instantiate(ProjectilePrefab, TreeSpawnpointRight.transform.position, TreeSpawnpointRight.transform.rotation, TreeSpawnpointRight.transform).GetComponent<Projectile>();
        tempProjectile.GetComponent<SpriteRenderer>().sortingOrder = 1;
        Projectiles.Add(tempProjectile);
        StartCoroutine(ShootWithDelay(tempProjectile));

        tempProjectile = Instantiate(ProjectilePrefab, TreeSpawnpointLeft.transform.position, TreeSpawnpointLeft.transform.rotation, TreeSpawnpointLeft.transform).GetComponent<Projectile>();
        tempProjectile.GetComponent<SpriteRenderer>().sortingOrder = 1;
        Projectiles.Add(tempProjectile);
        StartCoroutine(ShootWithDelay(tempProjectile));
    }
    IEnumerator ShootWithDelay(Projectile projectile)
    {
        yield return new WaitForSeconds(ShootDelayTime);
        projectile.transform.parent = null;
        Vector3 Direction = (PlayerController.Instance.transform.position - projectile.transform.position).normalized;
        projectile.transform.rotation = Quaternion.FromToRotation(transform.up, Direction);
        projectile.Shoot(Damage, Direction, BulletSpeed);
    }
    public void Awoken()
    {
        TreeAwoken = true;
    }
    public void Slept()
    {
        TreeAwoken = false;
    }
}
