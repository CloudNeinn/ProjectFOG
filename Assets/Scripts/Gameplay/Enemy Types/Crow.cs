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
    public float dirx;
    // Start is called before the first frame update
    void Start()
    {
        x = 1;
        //enemyStartingPosition = transform.position;
        //InvokeRepeating("UpdatePath", 0f, 0.5f);
        dirx = transform.localScale.x;
        numberOfCircles = 0;
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        if(inSight()) {
            lastPlayerPos = charCon.transform.position;
            isAlert = true;
            //target = new Vector2(lastPlayerPos.x + flySway, lastPlayerPos.y + heightAbovePlayer);
        }
        if(isAlert && !isAttacking/* && x == 1*/) {
            //if(transform.position.x >= lastPlayerPos.x) flySway *= -1;
            CircleAroundPlayer();
            //x = 2;
        }
        if(numberOfCircles >= 4)
        {
            isAttacking = true;
            target = lastPlayerPos;

        }
        if(dirx != transform.localScale.x) {
            dirx = transform.localScale.x;
            numberOfCircles++;
        }
/*
        if(!inSight() && isAlert)
        {
            isAttacking = true;
            target = lastPlayerPos;
        }*/

    }

    void FixedUpdate()
    {
        if(isAlert) {
            PathFollow();
        }
    }

    void CircleAroundPlayer()
    {
        //flySway *= -1;
        if(transform.position.x < lastPlayerPos.x)  
        {
            target = new Vector2(lastPlayerPos.x + flySway, lastPlayerPos.y + heightAbovePlayer);
            //numberOfCircles++;
        }
        else target = new Vector2(lastPlayerPos.x - flySway, lastPlayerPos.y + heightAbovePlayer);

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

    public void UpdatePath() 
    {
        if(seeker.IsDone() && target != null) seeker.StartPath(_enemyrb.position, target, OnPathComplete);
    }

    public new void OnPathComplete(Path p) 
    {

        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
            //CircleAroundPlayer();
            if(isAttacking) 
            {
                isAttacking = false;
                numberOfCircles = 0;
            }
            //numberOfCircles++;
        }

    }
}
