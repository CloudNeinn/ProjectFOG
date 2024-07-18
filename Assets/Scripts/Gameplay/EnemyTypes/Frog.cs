using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : EnemyBase, IJumpable, IGroundable
{
    [field: Header ("Jump Options")]
    [field: SerializeField] public float jumpCooldown { get; set; }
    [field: SerializeField] public float jumpTimer { get; set; }
    [field: SerializeField] public bool jumpEnabled { get; set; }
    [field: SerializeField] public float jumpModifier { get; set; }
    [SerializeField] public float jumpHeight;

    [field: Header ("Ground box options")]
    [field: SerializeField] public Vector3 isGroundedBox { get; set; }
    [field: SerializeField] public float isGroundedDistance { get; set; }
    [field: SerializeField] public LayerMask checkGroundLayer { get; set; }
    void Update()
    {
        if(jumpTimer > 0 && !jumpEnabled) jumpTimer -= Time.deltaTime;
        else jumpEnabled = true;
        Rotate();
    }
    void FixedUpdate()
    {
        if(isPatroling && canPatrol) Patrol();
    }

    public void Jump(float jumpStrength)
    {
        _enemyrb.velocity = new Vector2(moveSpeed * Mathf.Sign(PatrolPoints[currentPatrolPoint].transform.position.x - transform.position.x), jumpStrength);
        jumpTimer = jumpCooldown;
        jumpEnabled = false;
    }
    public void Patrol()
    {
        if(Vector2.Distance(transform.position, PatrolPoints[currentPatrolPoint].transform.position) <= 1.5f && isGrounded()){
            moveAfterStanding(((PatrolPoints[currentPatrolPoint].transform.position - transform.position).normalized));
            if(standingCooldown <= 0){
                if(currentPatrolPoint == numberOfPatrolPoints - 1) currentPatrolPoint = 0;
                else currentPatrolPoint++;
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            }
        }
        else
        {
            standingCooldown = standingTime;
            if(jumpEnabled && isGrounded()) Jump(jumpHeight);          
        }
    }
    
    public void moveAfterStanding(Vector2 dir)
    {
        if(standingCooldown <= 0) {
            if(jumpEnabled && isGrounded()) Jump(jumpHeight);
        }
        else 
        {
            Debug.Log("Standing");
            standingCooldown -= Time.deltaTime;
            Stand();
        }
    }
    public bool isGrounded()
    {
        return Physics2D.OverlapBox(new Vector2(_enemycol.bounds.center.x, _enemycol.bounds.center.y - isGroundedDistance), isGroundedBox, 0, checkGroundLayer);
    }    

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(_enemycol.bounds.center - transform.up * isGroundedDistance, isGroundedBox);  
    }
}
