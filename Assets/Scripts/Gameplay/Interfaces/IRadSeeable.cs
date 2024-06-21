using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRadSeeable : ISeeable
{
    float sightRadius { get; set; }
    Vector2 getVector();
}
