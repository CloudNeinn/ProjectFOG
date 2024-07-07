using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGroundable
{
    Vector3 isGroundedBox { get; set; }
    float isGroundedDistance { get; set; }
    LayerMask checkGroundLayer { get; set; }
    bool isGrounded();
}
