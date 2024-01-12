using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Dmg;
    public void Shoot(int dmg, Vector3 direction, float speed)
    {
        Dmg = dmg;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (direction.x >= 0) GetComponent<Animator>().SetFloat(GlobalConstants.HorizontalVelocity, 1);
        else GetComponent<Animator>().SetFloat(GlobalConstants.HorizontalVelocity, -1);
        rb.velocity = direction*speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GlobalConstants.Tags.Player.ToString()))
        {
            PlayerController.Instance.TakeDamage(Dmg);
            Destroy(this.gameObject);
        }
    }
}
