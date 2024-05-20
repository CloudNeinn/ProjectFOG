using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFlyable
{
    float moveSpeedX { get; set; }
    int flyDirection { get; set; }

    void Move(float speed, int direction);    
}
