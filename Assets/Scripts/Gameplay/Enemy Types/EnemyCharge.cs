using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharge : EnemyBase, ISeeable, IAttackable
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
        return Physics2D.OverlapBox(new Vector2(_enemycol.bounds.center.x + sightDistance * -transform.localScale.x,
         _enemycol.bounds.center.y), sightBoxSize, 0, playerLayer);
    }

    public bool inRange()
    {
        return Physics2D.OverlapBox(new Vector2(_enemycol.bounds.center.x + attackDistance * -transform.localScale.x,
         _enemycol.bounds.center.y), attackBoxSize, 0, playerLayer);
    }

    public bool isBehind()
    {
        return Physics2D.OverlapBox(new Vector2(_enemycol.bounds.center.x + behindDistance * -transform.localScale.x,
         _enemycol.bounds.center.y), behindBoxSize, 0, playerLayer);
    }

    public bool checkIfGround()
    {
        return Physics2D.OverlapBox(new Vector2(_enemycol.bounds.center.x + checkGroundDistance * -transform.localScale.x,
         _enemycol.bounds.center.y), checkGroundBoxSize, 0, checkGroundLayer);
    }

    public bool checkIfWall()
    {
        return Physics2D.OverlapBox(new Vector2(_enemycol.bounds.center.x + checkWallDistance * -transform.localScale.x,
         _enemycol.bounds.center.y), checkWallBoxSize, 0, checkWallLayer);
    }

    public bool isGrounded()
    {
        return Physics2D.OverlapBox(new Vector2(_enemycol.bounds.center.x, _enemycol.bounds.center.y - isGroundedDistance), isGroundedBox, 0, checkGroundLayer);
    }

    public bool _isGrounded;
    void Update()
    {
        _isGrounded = isGrounded();
        Rotate();
        Patrol();
        Attack();
    }

    public void Attack()
    {
        if(inSight())
        {
            if(noticeStandingCooldown > 0) 
            {
                noticeStandingCooldown -= Time.deltaTime;
                Stand();
            }
            else 
            {
                moveSpeed = attackSpeed;
            }
        }
        else
        {
            noticeStandingCooldown = noticeStandingTime;
            moveSpeed = patrolSpeed;
        } 
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(_enemycol.bounds.center - transform.up * isGroundedDistance, isGroundedBox);  

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_enemycol.bounds.center - transform.right * transform.localScale.x * attackDistance, attackBoxSize);      

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_enemycol.bounds.center - transform.right * transform.localScale.x * sightDistance, sightBoxSize);
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_enemycol.bounds.center - transform.right * transform.localScale.x * behindDistance, behindBoxSize);

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(_enemycol.bounds.center - transform.right * transform.localScale.x * checkGroundDistance, checkGroundBoxSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_enemycol.bounds.center - transform.right * transform.localScale.x * checkWallDistance, checkWallBoxSize);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius); 
    }
}
