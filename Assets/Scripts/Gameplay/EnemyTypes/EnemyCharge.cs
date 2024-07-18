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

    public new void Attack()
    {
        if(isBehind() && characterControl.Instance.moveSpeed != characterControl.Instance.crouchSpeed) ChangeDirection(-transform.localScale.x);
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
