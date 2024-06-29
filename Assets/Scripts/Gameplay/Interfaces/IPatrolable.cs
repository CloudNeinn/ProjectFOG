using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPatrolable
{
    GameObject LeftPoint { get; set; }
    GameObject RightPoint { get; set; }
    GameObject[] PatrolPoints { get; set; }
    int currentPatrolPoint { get; set; }
    int numberOfPatrolPoints { get; set; }
    float standingTime { get; set; }
    float standingCooldown { get; set; }
    bool canPatrol { get; set; }
    void Patrol();
    void moveAfterStanding(Vector2 dir);
}
