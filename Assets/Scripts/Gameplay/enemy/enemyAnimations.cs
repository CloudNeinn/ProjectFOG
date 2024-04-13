using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAnimations : MonoBehaviour
{   
    [Header ("Condition check")]
    public bool moves;
    [Header ("References")]
    public Animator anim;
    public Rigidbody2D enemyRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moves = isMoving();
        checkConditions();
    }
    public void startAttackAnimation(bool isStart)
    {
        anim.SetBool("isAttacking", isStart);
    }
    private bool isMoving()
    {
        if(enemyRigidbody.velocity.magnitude != 0) return true;
        else return false;
    }

    private void checkConditions()
    {
        if(isMoving()) anim.SetBool("isMoving", true);
        else anim.SetBool("isMoving", false);

       /* if(isJumping()) anim.SetBool("isJumping", true);
        else anim.SetBool("isJumping", false);

        if(isFalling()) anim.SetBool("isFalling", true);
        else anim.SetBool("isFalling", false);

        if(isGrounded()) anim.SetBool("isGrounded", true);
        else anim.SetBool("isGrounded", false);*/
    }
}
