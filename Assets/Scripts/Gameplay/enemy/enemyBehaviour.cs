using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class enemyBehaviour : MonoBehaviour
{
    [Header("Pathfinding")]
    public Vector3 target;
    public Transform playerPosition;
    public Vector3  enemyStartingPosition;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;
    public Vector3 startOffset;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheckOffset = 0.1f;
    public Vector3 boxSize;
    public float maxDistance;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public float jumpCooldown;
    public float jumpTimer;
    public bool directionLookEnabled = true;
    public bool highJumper;
    public bool _isGrounded;
    public bool isSensing;
    public bool isFlying;
    public bool notAtPoint;
    public bool moves;

    [Header("References")]
    private Path path;
    public int currentWaypoint = 0;
    public bool isGrounded;
    private Seeker seeker;
    private Rigidbody2D rb;
    private enemyVision eneVis;
    private enemyPatrol enePat;

public Vector2 force;
    public Vector2 direction;
    public void Start()
    {
        playerPosition = FindObjectOfType<characterControl>().transform;
        target = transform.position;
        enemyStartingPosition = transform.position;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        eneVis = GetComponent<enemyVision>();
        enePat = GetComponent<enemyPatrol>();
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("enemy"), LayerMask.NameToLayer("enemy"), true);
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.BoxCast(transform.position, boxSize, 0f, -transform.up, maxDistance, LayerMask.GetMask("ground"));
        _isGrounded = isGrounded;
        if(!(eneVis.isAttacking))
        {
            if(isSensing)
            {
                if (TargetInDistance() && followEnabled)
                {
                    PathFollow();
                }
            }
            else
            {
                if (eneVis.inSight && followEnabled)
                {
                    target = playerPosition.position;
                    enePat.enabled = false;
                    notAtPoint =true;
                    PathFollow();
                }

                if(!(eneVis.inSight) && followEnabled)
                {
                    target = enemyStartingPosition;
                    PathFollow();                
                }
                
            }
            if(enePat.LeftPoint != null && enePat.RightPoint != null )
            {
                if(!(eneVis.inSight) && enePat.LeftPoint.transform.position.x <= transform.position.x && 
                enePat.RightPoint.transform.position.x >= transform.position.x && 
                enePat.LeftPoint.transform.position.y + 1f >= transform.position.y && 
                enePat.RightPoint.transform.position.y - 1f <= transform.position.y)
                {
                    notAtPoint = false;
                    enePat.enabled = true;
                    this.enabled = false;
                    currentWaypoint = 0;
                }
            }
        }
    }

    private void UpdatePath()
    {
        if(!(eneVis.isAttacking))
        {
            if(isSensing)
            {
                if (followEnabled && TargetInDistance() && seeker.IsDone())
                {
                    seeker.StartPath(rb.position, target, OnPathComplete);
                }            
            }
            else
            {
                if (followEnabled && eneVis.inSight && seeker.IsDone())
                {
                    seeker.StartPath(rb.position, target, OnPathComplete);
                }     
                if(notAtPoint)
                {
                    if(!(eneVis.inSight) && followEnabled && seeker.IsDone())
                    {
                        seeker.StartPath(rb.position, target, OnPathComplete);             
                    }   
                }    
            }
        }
    }

    private void PathFollow()
    {   
        if(enePat.enabled == false || !(eneVis.isAttacking))
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

            // See if colliding with anything
            //startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);
            //isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.5f);
            
            // Direction Calculation
            direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            force = direction * speed * Time.deltaTime;

            // Jump
            if (jumpEnabled && isGrounded && target.y > transform.position.y && !isFlying && !(eneVis.isAttacking))
            {
                if (direction.y > jumpNodeHeightRequirement)
                {
                    rb.AddForce(Vector2.up * speed * getJumpModifier() );
                    jumpEnabled = false;
                    jumpTimer = jumpCooldown;
                }
            }

            if(!jumpEnabled)
            {
                jumpTimer -= Time.deltaTime;
                if(jumpTimer <= 0) jumpEnabled = true;
            }

            // Movement
            rb.AddForce(force);

            // Next Waypoint
            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }
        // Direction Graphics Handling
        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
        
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private float getJumpModifier()
    {
        if(target.y > transform.position.y && !highJumper) jumpModifier = Mathf.Sqrt((target.y - transform.position.y)*0.3f);
        return jumpModifier * 0.9f;
        //if(target.y > transform.position.y && !highJumper) jumpModifier = (((target.y - transform.position.y)*4.3f)/14.5f);
        //if(path.vectorPath[currentWaypoint].y > transform.position.y) jumpModifier = Mathf.Sqrt((path.vectorPath[currentWaypoint].y - transform.position.y)*0.2f);
        //return jumpModifier;
    }
}