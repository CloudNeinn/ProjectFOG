using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : EnemyChaser, IBlockable
{
    [field: Header ("Blocking")]
    [field: SerializeField] public bool blockHit { get; set;}
    [field: SerializeField] public bool isBlocking { get; set;}
    [field: SerializeField] public float blockTime { get; set;}
    [field: SerializeField] public float blockCooldown { get; set;}
    [field: SerializeField] public float afterHitTime { get; set;}
    [field: SerializeField] public float afterHitCooldown { get; set;}
    [field: SerializeField] public float blockingSpeed { get; set;}
    [field: SerializeField] public Vector3 blockingBoxSize { get; set;}
    [field: SerializeField] public Vector3 blockingBoxOffset { get; set;}
    [field: SerializeField] public LayerMask blockingLayers { get; set;}
    [field: SerializeField] public float maxBlockForce { get; set;}
    [field: SerializeField] public enemyHealthBlock eneHeaBlo { get; set;}
    public Vector3 speed;

    public void Start()
    {
        eneHeaBlo = GetComponent<enemyHealthBlock>();
        enemyStartingPosition = transform.position;
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void Update()
    {
        speed = _enemyrb.velocity;
        _isGrounded = isGrounded();
        //if(!canAttack) 
        Rotate();
        Attack();
        //if(followEnabled) PathFollow();
        if(isBlocking) Block();
    }
    
    public new void Attack()
    {
        if(inSight()) 
        {
            isAlert = true;
            forgetCooldown = forgetTime;
        }

        if(isBehind() && charCon.moveSpeed != charCon.crouchSpeed) {
            ChangeDirection(-transform.localScale.x);
            isAlert = true;
        }

        if(!isAlert && !canPatrol && Vector2.Distance(transform.position, enemyStartingPosition) >= 3f)
        {
            target = enemyStartingPosition;
            isPatroling = false;
            followEnabled = true;
        }
        else if(!isAlert) 
        {
            followEnabled = false;
            isPatroling = true;
        }

        if(isAlert) 
        {
            //moveSpeed = attackSpeed;
            isPatroling = false;
            target = playerPosition.position;
            if(!inRange() && !canAttack) 
            {
                followEnabled = true;
                isBlocking = true;
            }
            else 
            {
                Stand();
                followEnabled = false;
                //isBlocking = false;
            }
            
            if(canAttack && inRange())
            {
                //ani.SetTrigger("AttackBehavior");
                attackCooldown = attackTime;
                canAttack = false;//should become false on animation end
            }
            if(!canAttack && inRange())
            {
                if(attackCooldown < 0) 
                {
                    //isBlocking = false;//block removal animation
                    canAttack = true;//should become true on block removal animation end
                }
                else attackCooldown -= Time.deltaTime;
            }
        }
        else moveSpeed = patrolSpeed;

        if(!inSight() && isAlert)
        {
            if(forgetCooldown < 0) isAlert = false;
            else forgetCooldown -= Time.deltaTime;
        }
        else if(isAlert && Vector2.Distance(transform.position, enemyStartingPosition) >= 20f) isAlert = false;

    }
    
    public void Block()
    {
        moveSpeed = blockingSpeed;
        blockHit = inBlockingRange();
        if(blockHit && charCon.transform.position.x * Mathf.Sign(transform.localScale.x) >= 
            (transform.position.x + blockingBoxOffset.x * Mathf.Sign(transform.localScale.x)) * Mathf.Sign(transform.localScale.x)) eneHeaBlo.isBlocked = true;
    }

    public bool inBlockingRange()
    {
        return Physics2D.OverlapBox(transform.position + blockingBoxOffset * Mathf.Sign(transform.localScale.x), blockingBoxSize, 0, blockingLayers);
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

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + blockingBoxOffset * Mathf.Sign(transform.localScale.x), blockingBoxSize);
    }
}
