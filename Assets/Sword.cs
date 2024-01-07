using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public GameObject HitVFXPrefab;
    public int Damage = 50;
    List<AIController> monsters = new List<AIController>();
    public void Attack()
    {
        monsters.Clear();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GlobalConstants.Tags.Monster.ToString()))
        {
            if (monsters.Contains(collision.gameObject.GetComponent<AIController>())) return;
            if (collision.GetComponent<AIController>().IsDead) return;
            monsters.Add(collision.gameObject.GetComponent<AIController>());
            collision.gameObject.GetComponent<AIController>().TakeDmg(Damage);
            Destroy(Instantiate(HitVFXPrefab, collision.transform.position, Quaternion.identity), 1f);
        }
    }
}
