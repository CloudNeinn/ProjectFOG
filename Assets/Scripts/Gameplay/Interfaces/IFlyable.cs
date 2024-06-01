using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFlyable : IMoveable
{
    float flySpeed { get; set; }
    Vector2 flyDirection { get; set; }
    void Fly(float speed, Vector2 direction);    
}
