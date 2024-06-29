using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISeeable
{
    float noticeStandingTime { get; set; }
    float noticeStandingCooldown { get; set; }
    
    Vector3 behindBoxSize { get; set; }
    float behindDistance { get; set; }
    Vector3 isGroundedBox { get; set; }
    float isGroundedDistance { get; set; }

    Vector3 checkGroundBoxSize { get; set; }
    float checkGroundDistance { get; set; }
    Vector3 checkWallBoxSize { get; set; }
    float checkWallDistance { get; set; }
    LayerMask checkGroundLayer { get; set; }
    LayerMask checkWallLayer { get; set; }

    LayerMask playerLayer { get; set; }
    LayerMask raycastLayer { get; set; }

    float patrolSpeed { get; set; }
    float attackSpeed { get; set; }

    bool isAlert { get; set; }
    bool isSensing { get; set; }

    characterControl charCon { get; set; }

    bool inSight();
    bool inRange();
    bool isBehind();
    bool checkIfWall();
    bool checkIfGround();
    bool isGrounded();
}
