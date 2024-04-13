using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPatrolable
{
    GameObject LeftPoint { get; set; }
    GameObject RightPoint { get; set; }
    void Patrol();
}
