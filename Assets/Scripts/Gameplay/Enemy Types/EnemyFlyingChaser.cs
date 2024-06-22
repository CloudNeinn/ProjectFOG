using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyFlyingChaser : EnemyFlyingAttacker, IChaseable
{
    [field: Header ("Pathfinding")]
    [field: SerializeField] public Seeker seeker { get; set; }
    [field: SerializeField] public Path path { get; set; }
    [field: SerializeField] public int currentWaypoint { get; set; } 
    [field: SerializeField] public Vector3 target { get; set; } 

    [field: SerializeField] public Transform playerPosition { get; set; } 
    [field: SerializeField] public Vector3 enemyStartingPosition { get; set; } 
    [field: SerializeField] public float activateDistance { get; set; } //50f
    [field: SerializeField] public float pathUpdateSeconds { get; set; } //0.5f
 
    [field: SerializeField] public bool followEnabled { get; set; } 
    [field: SerializeField] public float nextWaypointDistance { get; set; } //3f
    [field: SerializeField] public float speed { get; set; } //200f
    [field: SerializeField] public Vector2 force { get; set; }
    [field: SerializeField] public Vector2 direction { get; set; }
    [field: SerializeField] public float forceMultiplier { get; set; } 

    void Awake()
    {
        enemyStartingPosition = transform.position;
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
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

    public void OnPathComplete(Path p) 
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
