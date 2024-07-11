using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable
{
    bool isStanding { get; set; }
    void Stand();
    void Rotate();
    void ChangeDirection(float direction);
}
