using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharge : EnemyAttacker
{

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
