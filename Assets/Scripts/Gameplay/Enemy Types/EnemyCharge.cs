using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharge : EnemyAttacker
{
    public bool inSight()
    {
        return Physics2D.OverlapBox(new Vector2(_enemycol.bounds.center.x + sightDistance * -transform.localScale.x,
         _enemycol.bounds.center.y), sightBoxSize, 0, playerLayer);
    }

    public bool inRange()
    {
        return Physics2D.OverlapBox(new Vector2(_enemycol.bounds.center.x + attackDistance * -transform.localScale.x,
         _enemycol.bounds.center.y), attackBoxSize, 0, playerLayer);
    }

    public bool isBehind()
    {
        return Physics2D.OverlapBox(new Vector2(_enemycol.bounds.center.x + behindDistance * -transform.localScale.x,
         _enemycol.bounds.center.y), behindBoxSize, 0, playerLayer);
    }

    public bool checkIfGround()
    {
        return Physics2D.OverlapBox(new Vector2(_enemycol.bounds.center.x + checkGroundDistance * -transform.localScale.x,
         _enemycol.bounds.center.y), checkGroundBoxSize, 0, checkGroundLayer);
    }

    public bool checkIfWall()
    {
        return Physics2D.OverlapBox(new Vector2(_enemycol.bounds.center.x + checkWallDistance * -transform.localScale.x,
         _enemycol.bounds.center.y), checkWallBoxSize, 0, checkWallLayer);
    }

    public bool isGrounded()
    {
        return Physics2D.OverlapBox(new Vector2(_enemycol.bounds.center.x, _enemycol.bounds.center.y - isGroundedDistance), isGroundedBox, 0, checkGroundLayer);
    }

    void Update()
    {
        _isGrounded = isGrounded();
        Rotate();
        //Patrol();
        Attack();
    }

    public void Attack()
    {
        if(isBehind() && charCon.moveSpeed != charCon.crouchSpeed) ChangeDirection(-transform.localScale.x);
        if(inSight())
        {
            if(noticeStandingCooldown > 0) 
            {
                noticeStandingCooldown -= Time.deltaTime;
                Stand();
            }
            else 
            {
                moveSpeed = attackSpeed;
            }
        }
        else
        {
            noticeStandingCooldown = noticeStandingTime;
            moveSpeed = patrolSpeed;
        } 
        
    }
}
