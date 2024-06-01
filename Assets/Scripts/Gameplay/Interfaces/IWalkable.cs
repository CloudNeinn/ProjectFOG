using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWalkable : IMoveable
{
    float moveSpeed { get; set; }
    int directionX { get; set; }
    void Walk(int direction);
}
