using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable
{
    void Stand();
    void Rotate();
    void ChangeDirection(float direction);
}
