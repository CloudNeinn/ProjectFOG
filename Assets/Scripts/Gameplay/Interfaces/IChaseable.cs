using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public interface IChaseable
{
    Path path { get; set; }
    Seeker seeker { get; set; }
    int currentWaypoint { get; set; } 

    Vector3 target { get; set; } 
    Transform playerPosition { get; set; } 
    Vector3  enemyStartingPosition { get; set; } 
    float activateDistance { get; set; } //50f
    float pathUpdateSeconds { get; set; } //0.5f

    bool followEnabled { get; set; } 
    bool jumpEnabled { get; set; } 
    float jumpModifier { get; set; } // 0.3f
    float jumpNodeHeightRequirement { get; set; } //0.8f
    float nextWaypointDistance { get; set; } //3f
    float speed { get; set; } //200f

    void PathFollow();
    void UpdatePath();
    void OnPathComplete(Path p);
}
