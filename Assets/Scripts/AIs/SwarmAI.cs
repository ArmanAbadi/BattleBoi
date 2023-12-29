using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmAI : AIController
{
    protected override IEnumerator MovementUpdate()
    {
        while (!IsDead)
        {
            UpdateDirection();
            UpdateMovement();

            yield return new WaitForSeconds(0);
        }
    }
    protected override void UpdateDirection()
    {
        Direction = Vector3.zero;
        /*RaycastHit2D hit = Physics2D.CircleCast(transform.position, AvoidanceRange, Vector2.zero, 1);

        if (hit.transform.gameObject.CompareTag(GlobalConstants.Tags.Monster.ToString()))
        {
            if ((transform.position - hit.transform.position).magnitude < AvoidanceRange)
            {
                //Direction = (transform.position - hit.transform.position).normalized/3f;
            }
        }*/
        BasicFollow();

        animator.SetFloat(GlobalConstants.HorizontalVelocity, Direction.x);
    }
    protected override void UpdateMovement()
    {
        rigidbody2D.velocity = Speed * Direction.normalized;
        //if (Direction != Vector3.zero) transform.rotation = Quaternion.LookRotation(Vector3.forward, -Direction);
    }
}
