using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlying : EnemyBase, IFlyable
{
    [field: SerializeField] public float flySpeed { get; set; }
    [field: SerializeField] public Vector2 flyDirection { get; set; }
    public void Fly(float speed, Vector2 direction)
    {
        _enemyrb.velocity = new Vector2(speed * direction.x, speed * direction.y);
    }   
    public void Stand() 
    {
        _enemyrb.velocity = new Vector2(0, 0);
    } 
    public void Patrol()
    {
        if(Vector2.Distance(transform.position, PatrolPoints[currentPatrolPoint].transform.position) <= 0.5f){
            moveAfterStanding(flyDirection);
            if(standingCooldown <= 0){
                if(currentPatrolPoint == numberOfPatrolPoints - 1) currentPatrolPoint = 0;
                else currentPatrolPoint++;
            }
        }
        else
        {
            standingCooldown = standingTime;
            Fly(moveSpeed, flyDirection);          
        }
    }
    public void moveAfterStanding(Vector2 dir)
    {
        if(standingCooldown <= 0) {
            Fly(moveSpeed, dir);
        }
        else 
        {
            Debug.Log("Standing");
            standingCooldown -= Time.deltaTime;
            Stand();
        }
    }    
    
    public void Rotate()
    {
        if (_enemyrb.velocity.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (_enemyrb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        flyDirection = ((PatrolPoints[currentPatrolPoint].transform.position - transform.position).normalized);
    }

    void Update()
    {
        Rotate();
    }

    void FixedUpdate()
    {
        if(isPatroling) Patrol();
    }
}