using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlying : EnemyChaser, IFlyable
{
    [field: SerializeField] public float moveSpeedX { get; set; }
    [field: SerializeField] public int flyDirection { get; set; }
    public void Move(float speed, int direction) {}
}
