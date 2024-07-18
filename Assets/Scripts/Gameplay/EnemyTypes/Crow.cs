using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Crow : EnemyFlyingChaser
{
    [Header ("Crow Options")]
    public float heightAbovePlayer;
    public float nestRadius;
    public float flySway;
    public Vector2 lastPlayerPos;
    public Vector2 destination;
    public Vector2 target;
    public bool isAttacking;
    public bool returnHome;
    public int numberOfCircles;
    public LayerMask crowNestLayer;
    public LayerMask ceilingLayer;
    public float maxHeight;
    
    // Start is called before the first frame update
    void Start()
    {
        CheckCeiling();
    }

    // Update is called once per frame
    void Update()
    {
        //destination = target;
        //if is grounded standing animation
        Rotate();
        if(inNestRange())
        {
            if(inSight()) lastPlayerPos = characterControl.Instance.transform.position;
            if(inSight() && !isAlert)
            {
                isAlert = true;
                InitialDirectionDetermination();
            }
            if(isAlert && !isAttacking) ChangeDestination();
            if(isAttacking) Attack();
            CheckCeiling();
        }
        else 
        {
            isAlert = false;
            returnHome = true;
            target = enemyStartingPosition;
        }

        if(returnHome && Vector2.Distance(target, transform.position) <= 1.5f) 
        {
            returnHome = false;
            _enemyrb.velocity = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        if(isAlert || returnHome) PathFollow();
    }

    void Attack()
    {
        target = lastPlayerPos;
        if(Vector2.Distance(target, transform.position) <= 5f) isAttacking = false;
    }

    void DestinationDetermination()
    {
        target.Set(lastPlayerPos.x + flySway, ReturnUpToMax(maxHeight,lastPlayerPos.y + Random.Range(heightAbovePlayer * 0.8f,heightAbovePlayer * 1.5f)));
    }

    void InitialDirectionDetermination()
    {
        if(transform.position.x > lastPlayerPos.x) flySway *= -1;
        DestinationDetermination();
    }

    void ChangeDestination()
    {
        if(Vector2.Distance(target, transform.position) <= 2.5f) 
        {
            flySway = -flySway;
            numberOfCircles++;
            if(numberOfCircles >= 5) 
            {
                isAttacking = true;
                numberOfCircles = 0;
            }
            DestinationDetermination();
        }
    }

    bool inNestRange()
    {
        return Physics2D.OverlapCircle(transform.position, nestRadius, crowNestLayer);
    }

    void CheckCeiling()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0,1f), 100.0f, ceilingLayer);
        Debug.DrawRay(transform.position, new Vector2(0,1f), Color.red, 0f);
        maxHeight = hit.point.y - 3f;
    }

    public float ReturnUpToMax(float max, float value)
    {
        if(value < max) return value;
        else return max;
    }
/*
    public new void PathFollow() 
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
        force = direction * (moveSpeed * forceMultiplier) * Time.deltaTime;

        // Movement
        _enemyrb.AddForce(force);

        // Next Waypoint
        float distance = Vector2.Distance(_enemyrb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }    
    }

    public new void UpdatePath() 
    {
        if(seeker.IsDone() && target != null) seeker.StartPath(_enemyrb.position, target, OnPathComplete);
    }

    public new void OnPathComplete(Path p) 
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }*/
}
