using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public GameObject HitVFXPrefab;
    public int Damage = 50;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GlobalConstants.Tags.Monster.ToString()))
        {
            if (collision.GetComponent<AIController>().IsDead) return;
            collision.gameObject.GetComponent<AIController>().TakeDmg(Damage);
            Destroy(Instantiate(HitVFXPrefab, collision.transform.position, Quaternion.identity), 1f);
        }
    }
}
