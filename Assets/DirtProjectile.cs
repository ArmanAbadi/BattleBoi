using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtProjectile : Projectile
{
    public override void Shoot(int dmg, Vector3 direction, float speed)
    {
        Dmg = dmg;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (direction.x >= 0) GetComponent<Animator>().SetFloat(GlobalConstants.HorizontalVelocity, 1);
        else GetComponent<Animator>().SetFloat(GlobalConstants.HorizontalVelocity, -1);
        rb.velocity = direction * speed;
    }
}
