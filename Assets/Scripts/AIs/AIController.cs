using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public int MaxHealth = 100;
    public int CurrentHealth;
    public float MovementUpdateTime = 1f;
    protected Vector3 Direction = Vector2.zero;
    public float Speed = 2f;
    protected Rigidbody2D rigidbody2D;
    protected Animator animator;
    public bool IsDead = false;
    protected float AggroRange = 4f;
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        StartCoroutine(MovementUpdate());
    }

    // Update is called once per frame
    protected virtual IEnumerator MovementUpdate()
    {
        while (!IsDead)
        {
            UpdateDirection();
            UpdateMovement();

            yield return new WaitForSeconds(MovementUpdateTime);
        }
    }
    protected virtual void UpdateDirection()
    {
        Direction = new Vector3(Random.Range(-1,2), Random.Range(-1,2),0);
        animator.SetFloat(GlobalConstants.HorizontalVelocity, Direction.x);
    }
    protected virtual void UpdateMovement()
    {
        rigidbody2D.velocity = Speed * Direction.normalized;
        //if (Direction != Vector3.zero) transform.rotation = Quaternion.LookRotation(Vector3.forward, -Direction);
    }
    protected virtual void BasicFollow()
    {
        if ((PlayerController.Instance.transform.position - transform.position).magnitude < AggroRange)
        {
            Direction += (PlayerController.Instance.transform.position - transform.position).normalized;
        }
    }
    protected void RandomWalk()
    {
        Direction = new Vector3(Random.Range(-1f, 2f), Random.Range(-1f, 2f), 0);
    }
    protected void Attack()
    {
    }
    protected void Death()
    {
        rigidbody2D.velocity = Vector2.zero;
        IsDead = true;
        animator.SetTrigger(GlobalConstants.DeadTrigger);
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
    public virtual void TakeDmg(int dmg)
    {
        if (IsDead) return;

        CurrentHealth -= dmg;
        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Death();
        }
    }
}
