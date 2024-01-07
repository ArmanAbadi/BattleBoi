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
            Debug.Log("pull");
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
            Debug.Log("bro " + PullTime);
            t += Time.deltaTime / PullTime;

            if (t > 1) t = 1;
            Debug.Log("bro "+ Time.deltaTime);
            dropable.transform.position = Vector3.Lerp(start, target, t);

            yield return null;
        }
    }
}
