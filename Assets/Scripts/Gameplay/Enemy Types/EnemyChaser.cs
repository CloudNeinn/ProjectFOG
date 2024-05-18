using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyChaser : EnemyCharge, IJumpable, IChaseable
{
    [field: Header ("Pathfinding")]
    [field: SerializeField] public Path path { get; set; }
    [field: SerializeField] public Seeker seeker { get; set; }
    [field: SerializeField] public int currentWaypoint { get; set; } 
    [field: SerializeField] public Vector3 target { get; set; } 

    [field: SerializeField] public Transform playerPosition { get; set; } 
    [field: SerializeField] public Vector3 enemyStartingPosition { get; set; } 
    [field: SerializeField] public float activateDistance { get; set; } //50f
    [field: SerializeField] public float pathUpdateSeconds { get; set; } //0.5f
 
    [field: SerializeField] public bool followEnabled { get; set; } 
    [field: SerializeField] public bool jumpEnabled { get; set; } 
    [field: SerializeField] public float jumpModifier { get; set; } // 0.3f
    [field: SerializeField] public float jumpNodeHeightRequirement { get; set; } //0.8f
    [field: SerializeField] public float nextWaypointDistance { get; set; } //3f
    [field: SerializeField] public float speed { get; set; } //200f

    [field: SerializeField] public float jumpCooldown { get; set; }
    [field: SerializeField] public float jumpTimer { get; set; }
    public Vector2 force;
    public Vector2 direction;
    void Start()
    {
        directionX = (int) transform.localScale.x;
        enemyStartingPosition = transform.position;
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void Update()
    {
        _isGrounded = isGrounded();
        Rotate();
        Attack();
    }

    public void Attack()
    {
        if(inSight()) 
        {
            isAlert = true;
            forgetCooldown = forgetTime;
        }

        if(isBehind() && charCon.moveSpeed != charCon.crouchSpeed) {
            ChangeDirection(-transform.localScale.x);
            isAlert = true;
        }

        if(!isAlert && 
        (LeftPoint.transform.position.x - 0.5f >= transform.position.x || 
        RightPoint.transform.position.x + 0.5f <= transform.position.x) && 
        (LeftPoint.transform.position.y + 1.5f <= transform.position.y || 
        RightPoint.transform.position.y - 1.5f >= transform.position.y))
        {
            target = enemyStartingPosition;
            isPatroling = false;
            PathFollow();
        }
        else if(!isAlert) isPatroling = true; //Patrol();

        if(isAlert) 
        {
            moveSpeed = attackSpeed;
            isPatroling = false;
            target = playerPosition.position;
            PathFollow();
        }
        else moveSpeed = patrolSpeed;

        if(isAlert)
        {/*
            if(canAttack)
            {
                //ani.SetTrigger("AttackBehavior");
                attackCooldown = attackTime;
                Stand();
            }
            else
            {
                if(attackCooldown < 0) canAttack = true;
                else attackCooldown -= Time.deltaTime;
                if(!checkIfWall() && checkIfGround()) 
                {
                    //Move(getDirectionAwayFromPlayer());
                    //ChangeDirection(-getDirectionAwayFromPlayer());
                }
            }*/
        }
        if(!inSight() && isAlert)
        {
            if(forgetCooldown < 0) isAlert = false;
            else forgetCooldown -= Time.deltaTime;
        }
    }

    public void PathFollow() 
    {
        if (path == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }  

        direction = ((Vector2)path.vectorPath[currentWaypoint] - _enemyrb.position).normalized;
        force = direction * (moveSpeed * 24200) * Time.deltaTime;

        // Jump
        if (jumpEnabled && isGrounded() && target.y > transform.position.y 
         && direction.y > jumpNodeHeightRequirement) Jump(getJumpModifier());

        if(!jumpEnabled)
        {
            jumpTimer -= Time.deltaTime;
            if(jumpTimer <= 0) jumpEnabled = true;
        }

        // Movement
        _enemyrb.AddForce(force);

        // Next Waypoint
        float distance = Vector2.Distance(_enemyrb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }    
    }

    public void UpdatePath() 
    {
        if(seeker.IsDone() && target != null) seeker.StartPath(_enemyrb.position, target, OnPathComplete);
    }

    public void OnPathComplete(Path p) 
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public float getJumpModifier() 
    {
        if(target.y > transform.position.y) jumpModifier = Mathf.Sqrt((target.y - transform.position.y)*0.3f);
        return jumpModifier * 0.9f;
    }

    public void Jump(float jumpStrength) 
    {
        _enemyrb.AddForce(Vector2.up * speed * jumpStrength);
        jumpEnabled = false;
        jumpTimer = jumpCooldown;
    }

}
