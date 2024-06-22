using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Crow : EnemyFlyingChaser
{
    [Header ("Crow Options")]
    public float heightAbovePlayer;
    public float radius;
    public float flySway;
    public bool start;
    public Vector2 lastPlayerPos;
    public bool isAttacking;
    public int numberOfCircles;
    public int x;
    // Start is called before the first frame update
    void Start()
    {
        x = 1;
        //enemyStartingPosition = transform.position;
        //InvokeRepeating("UpdatePath", 0f, 0.5f);
        numberOfCircles = 0;
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        if(inSight()) {
            CircleAroundPlayer();
            lastPlayerPos = charCon.transform.position;
            isAlert = true;
        }

        if(isAttacking) {
            target = lastPlayerPos;
            numberOfCircles = 0;
        }

    }

    void FixedUpdate()
    {
        if(isAlert) {
            PathFollow();
        }
    }

    void CircleAroundPlayer()
    {
        if(transform.position.x < lastPlayerPos.x && !isAttacking)  target = new Vector2(lastPlayerPos.x + flySway, lastPlayerPos.y + Random.Range(heightAbovePlayer * 0.8f,heightAbovePlayer * 1.5f));
        else if(!isAttacking) target = new Vector2(lastPlayerPos.x - flySway, lastPlayerPos.y + Random.Range(heightAbovePlayer * 0.8f,heightAbovePlayer * 1.5f));
    }

    public new void PathFollow() 
    {
        if (path == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            CircleAroundPlayer();
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
            if(isAttacking) isAttacking = false;
            path = p;
            currentWaypoint = 0;
            numberOfCircles++;
            if(numberOfCircles >= 12) {
                CircleAroundPlayer();
                isAttacking = true;
            }
        }

    }
}
