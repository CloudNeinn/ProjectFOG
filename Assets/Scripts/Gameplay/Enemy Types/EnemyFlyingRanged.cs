using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingRanged : EnemyFlying, ISeeable, IAttackable
{
    [field: Header ("Attack Options")]
    [field: SerializeField] public float attackTime { get; set; }
    [field: SerializeField] public float attackCooldown { get; set; }
    [field: SerializeField] public float forgetTime { get; set; }
    [field: SerializeField] public float forgetCooldown { get; set; }
    [field: SerializeField] public float damage { get; set; }
    [field: SerializeField] public float knockStrengthX { get; set; }
    [field: SerializeField] public float knockStrengthY { get; set; }
    [field: SerializeField] public float blockKnockStrengthX { get; set; }
    [field: SerializeField] public float blockKnockStrengthY { get; set; }    
    [field: SerializeField] public bool canAttack { get; set; }
    [field: SerializeField] public bool isAlert { get; set; }

    [field: Header ("Additional Movement Options")]
    [field: SerializeField] public float patrolSpeed { get; set; }
    [field: SerializeField] public float attackSpeed { get; set; }
    [field: SerializeField] public float noticeStandingTime { get; set; }
    [field: SerializeField] public float noticeStandingCooldown { get; set; }

    [field: Header ("Check Box Options")]
    [field: SerializeField] public Vector3 attackBoxSize { get; set; }
    [field: SerializeField] public float attackDistance { get; set; }
    [field: SerializeField] public Vector3 sightBoxSize { get; set; }
    [field: SerializeField] public float sightDistance { get; set; }
    [field: SerializeField] public Vector3 behindBoxSize { get; set; }
    [field: SerializeField] public float behindDistance { get; set; }
    [field: SerializeField] public float radius { get; set; }
    [field: SerializeField] public Vector3 checkGroundBoxSize { get; set; }
    [field: SerializeField] public float checkGroundDistance { get; set; }
    [field: SerializeField] public Vector3 checkWallBoxSize { get; set; }
    [field: SerializeField] public float checkWallDistance { get; set; }
    [field: SerializeField] public Vector3 isGroundedBox { get; set; }
    [field: SerializeField] public float isGroundedDistance { get; set; }

    [field: Header ("Layer Masks")]
    [field: SerializeField] public LayerMask playerLayer { get; set; }
    [field: SerializeField] public LayerMask raycastLayer { get; set; }
    [field: SerializeField] public LayerMask checkGroundLayer { get; set; }
    [field: SerializeField] public LayerMask checkWallLayer { get; set; }

    [field: Header ("Character References")]
    [field: SerializeField] public characterControl charCon { get; set; }

    public bool inSight()
    { 
        return false;
    }
    public bool inRange()
    { 
        return false;
    }    
    public bool isBehind()
    { 
        return false;
    }
    public bool checkIfWall()
    { 
        return false;
    }
    public bool checkIfGround()
    { 
        return false;
    }
    public bool isGrounded()
    { 
        return false;
    }
    public void Attack()
    { 
        
    }
}
