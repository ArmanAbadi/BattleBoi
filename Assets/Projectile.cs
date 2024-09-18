using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Dmg;
    public virtual void Shoot(int dmg, Vector3 direction, float speed)
    {
        Dmg = dmg;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized*speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GlobalConstants.Tags.Player.ToString()) && collision.gameObject.GetComponent<NetworkBehaviour>().HasInputAuthority)
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(Dmg);
            Destroy(this.gameObject);
        }
    }
}
