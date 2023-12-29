using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigController : AIController
{
    public float AggroAlliesRange = 10f;
    protected bool Aggrod = false;
    public float AggroSpeed = 5f;
    new public float MovementUpdateTime = 1f;

    protected override IEnumerator MovementUpdate()
    {
        while (!IsDead)
        {
            UpdateDirection();
            UpdateMovement();

            yield return new WaitForSeconds(Aggrod? 0:MovementUpdateTime);
        }
    }
    protected override void UpdateDirection()
    {
        Direction = Vector3.zero;
        if (Aggrod)
        {
            BasicFollow();
        }
        else
        {
            RandomWalk();
        }
        
        animator.SetFloat(GlobalConstants.HorizontalVelocity, Direction.x);
    }
    protected override void UpdateMovement()
    {
        rigidbody2D.velocity = Random.Range(0f, Aggrod? AggroSpeed:Speed) * Direction.normalized;
    }
    public override void TakeDmg(int dmg)
    {
        if (IsDead) return;
        Aggrod = true;
        AggroAllies();
        CurrentHealth -= dmg;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Death();
        }
    }
    void AggroAllies()
    {
        foreach(PigController pig in MonsterManager.Pigs)
        {
            if ((transform.position - pig.transform.position).magnitude < AggroAlliesRange)
            {
                pig.Aggrod = true;
            }
        }
    }
    protected override void BasicFollow()
    {
        Direction += (PlayerController.Instance.transform.position - transform.position).normalized;
    }
}
