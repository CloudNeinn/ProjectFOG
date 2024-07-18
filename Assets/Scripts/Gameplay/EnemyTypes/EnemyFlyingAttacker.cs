using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingAttacker : EnemyFlyingBase, IRadSeeable, IAttackable
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
    [field: SerializeField] public bool isSensing { get; set; }
    [field: SerializeField] private Vector3 _playerPosition;
    [field: SerializeField] private Vector3 _enemyPosition;

    [field: Header ("Additional Movement Options")]
    [field: SerializeField] public float patrolSpeed { get; set; }
    [field: SerializeField] public float attackSpeed { get; set; }
    [field: SerializeField] public float noticeStandingTime { get; set; }
    [field: SerializeField] public float noticeStandingCooldown { get; set; }

    [field: Header ("Check Box Options")]
    [field: SerializeField] public float sightRadius { get; set; }
    [field: SerializeField] public Vector3 behindBoxSize { get; set; }
    [field: SerializeField] public float behindDistance { get; set; }
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

    public bool inSight()
    {        
        if(inRange()) 
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, getVector(), 100.0f, raycastLayer);
            Debug.DrawRay(transform.position, getVector() * hit.distance, Color.red, 0f);
            if(hit.collider.name == "Charachter") return true;
            else return false;
        }
        else return false;
    }

    public bool inRange()
    {
        if((transform.localScale.x > 0 && transform.position.x < characterControl.Instance.transform.position.x || 
        transform.localScale.x < 0 && transform.position.x > characterControl.Instance.transform.position.x) && !isSensing) return Physics2D.OverlapCircle(transform.position, sightRadius, playerLayer);
        else if (isSensing) return Physics2D.OverlapCircle(transform.position, sightRadius, playerLayer);
        else return false;
    }

    public bool isBehind()
    {
        return Physics2D.OverlapBox(new Vector2(_enemycol.bounds.center.x + behindDistance * -transform.localScale.x,
         _enemycol.bounds.center.y), behindBoxSize, 0, playerLayer);
    }
    public Vector2 getVector()
    {
        if(!characterControl.Instance) characterControl.Instance = GameObject.FindObjectOfType<characterControl>();
        _playerPosition = characterControl.Instance.transform.position;
        _enemyPosition = transform.position;
        return new Vector2(_playerPosition.x - _enemyPosition.x, _playerPosition.y - _enemyPosition.y).normalized;
    }

    public bool checkIfGround()
    {
        return false;
    }

    public bool checkIfWall()
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(_enemycol.bounds.center - transform.up * isGroundedDistance, isGroundedBox);  

        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireCube(_enemycol.bounds.center - transform.right * transform.localScale.x * attackDistance, attackBoxSize);      

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireCube(_enemycol.bounds.center - transform.right * transform.localScale.x * sightDistance, sightBoxSize);
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_enemycol.bounds.center - transform.right * transform.localScale.x * behindDistance, behindBoxSize);

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(_enemycol.bounds.center - transform.right * transform.localScale.x * checkGroundDistance, checkGroundBoxSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_enemycol.bounds.center - transform.right * transform.localScale.x * checkWallDistance, checkWallBoxSize);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, sightRadius); 
    }
}