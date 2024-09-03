using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManController : AIController
{

    public float AttackRange = 2f;
    public float DeAggroRange = 5f;
    public int Damage = 10;
    bool TreeAwoken = false;
    public GameObject ProjectilePrefab;

    public float BulletSpeed = 3f;

    List<Projectile> Projectiles = new List<Projectile>();

    public GameObject TreeSpawnpointTop;
    public GameObject TreeSpawnpointLeft;
    public GameObject TreeSpawnpointRight;

    public float ShootDelayTime = 1f;
    protected override void UpdateDirection()
    {
            Direction = Vector3.zero;

            if (PlayerDistance() > DeAggroRange)
            {
                animator.SetBool(GlobalConstants.Aggro, false); 
            }
            else if (PlayerDistance() < AttackRange && ReadyToAttack() && TreeAwoken)
            {
                AttackCoolDownMarker = Time.time;
                if (HasStateAuthority)
                {
                    RPC_SpawnProjectile();
                }
                return;
            }
            else if(PlayerDistance() < AggroRange)
            {
                animator.SetBool(GlobalConstants.Aggro, true);
                Direction = BasicFollowDirection();
            }
    }
    protected override void UpdateMovement()
    {

    }
    protected override void Death()
    {
        for(int i = Projectiles.Count-1; i >= 0; i--)
        {
            if (Projectiles != null && Projectiles[i] != null && Projectiles[i].GetComponent<Rigidbody2D>().velocity.magnitude == 0)
            {
                Destroy(Projectiles[i].gameObject);
                Projectiles.RemoveAt(i);
            }
        }
        IsDead = true;
        animator.SetTrigger(GlobalConstants.DeadTrigger);
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
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    void RPC_SpawnProjectile()
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
        if (projectile == null) yield break;
        projectile.transform.parent = null;
        Vector3 Direction = (PlayerController.Instance.transform.position - projectile.transform.position).normalized;
        projectile.transform.rotation = Quaternion.FromToRotation(transform.up, Direction);
        projectile.GetComponent<BoxCollider2D>().enabled = true;
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
