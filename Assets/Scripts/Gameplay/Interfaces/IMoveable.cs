using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable
{
    float standingTime { get; set; }
    float standingCooldown { get; set; }
    float moveSpeed { get; set; }
    int directionX { get; set; }

    void Move(int direction);
    void Stand();
    void Rotate();
    void ChangeDirection(float direction);
}
