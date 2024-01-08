using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAttractor : MonoBehaviour
{
    List<GameObject> gameObjects = new List<GameObject>();
    public float PullTime = 0.5f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GlobalConstants.Tags.Dropable.ToString()))
        {
            StartCoroutine(PullDropable(collision.gameObject));
        }
    }
    IEnumerator PullDropable(GameObject dropable)
    {
        var t = 0f;
        var start = dropable.transform.position;
        var target = gameObject.transform.position;

        while (t < 1)
        {
            if(dropable == null) yield break;
            target = gameObject.transform.position;

            t += Time.deltaTime / PullTime;
            if (t > 1) t = 1;
            dropable.transform.position = Vector3.Lerp(start, target, t);

            yield return null;
        }
    }
}
