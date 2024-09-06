using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigMainMenuController : MonoBehaviour
{
    public float Speed;
    public float DirectionChangeTime;

    public GameObject Ham;

    Rigidbody2D Rigidbody;
    Animator Animator;

    public GameObject HitVFXPrefab;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        StartCoroutine(Move());
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit)
            {
                Clicked(hit.point);
            }
        }
    }
    IEnumerator Move()
    {
        while (true)
        {
            Rigidbody.velocity = Speed * new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0);

            Animator.SetFloat(GlobalConstants.HorizontalVelocity, Math.Sign(Rigidbody.velocity.x));

            yield return new WaitForSeconds(DirectionChangeTime);
        }
    }
    public void Clicked(Vector3 point)
    {
        GameObject temp = Instantiate(Ham, transform.position, transform.rotation);
        temp.transform.localScale *= 10;

        temp = Instantiate(HitVFXPrefab, point, Quaternion.identity);
        temp.transform.localScale *= 10;
        Destroy(gameObject);
    }
}
