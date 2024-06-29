using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : EnemyBase, IRadSeeable, IAttackable
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


    [field: Header ("Check Box Options")]
    [field: SerializeField] public Vector3 behindBoxSize { get; set; }
    [field: SerializeField] public float behindDistance { get; set; }
    [field: SerializeField] public float sightRadius { get; set; }
    [field: SerializeField] public Vector3 checkGroundBoxSize { get; set; }
    [field: SerializeField] public float checkGroundDistance { get; set; }
    [field: SerializeField] public Vector3 checkWallBoxSize { get; set; }
    [field: SerializeField] public float checkWallDistance { get; set; }
    [field: SerializeField] public Vector3 isGroundedBox { get; set; }
    [field: SerializeField] public float isGroundedDistance { get; set; }
    

    [field: Header ("Additional Movement Options")]
    [field: SerializeField] public float patrolSpeed { get; set; }
    [field: SerializeField] public float attackSpeed { get; set; }
    [field: SerializeField] public float noticeStandingTime { get; set; }
    [field: SerializeField] public float noticeStandingCooldown { get; set; }

    [field: Header ("Layer Masks")]
    [field: SerializeField] public LayerMask playerLayer { get; set; }
    [field: SerializeField] public LayerMask raycastLayer { get; set; }
    [field: SerializeField] public LayerMask checkGroundLayer { get; set; }
    [field: SerializeField] public LayerMask checkWallLayer { get; set; }

    [field: Header ("Character References")]
    [field: SerializeField] public GameObject projectile;
    [field: SerializeField] public Animator ani;
    [field: SerializeField] public characterControl charCon { get; set; }
    public bool _isBehind;

    void Start()
    {
        charCon = GameObject.FindObjectOfType<characterControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlert) Rotate();
        Attack();
        _isBehind = isBehind();
    }

    public void Attack()
    {
        if(inSight()) 
        {
            isAlert = true;
            forgetCooldown = forgetTime;
        }
        else if(isBehind() && charCon.moveSpeed != charCon.crouchSpeed) 
        {
            isAlert = true;
            ChangeDirection((int)(-transform.localScale.x));
        }

        if(isAlert)
        {
            isPatroling = false;
            if(canAttack)
            {
                ani.SetTrigger("AttackBehavior");
                attackCooldown = attackTime;
                Stand();
            }
            else 
            {
                if(attackCooldown < 0) canAttack = true;
                else attackCooldown -= Time.deltaTime;
                if(!checkIfWall() && checkIfGround()) 
                {
                    Walk(getDirectionAwayFromPlayer());
                    ChangeDirection(-getDirectionAwayFromPlayer());
                }
            }
        }
        else isPatroling = true;
        if(!inSight() && isAlert)
        {
            if(forgetCooldown < 0) isAlert = false;
            else forgetCooldown -= Time.deltaTime;
        }
        
    }

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

    public void spawnProjectile()
    {
        Instantiate(projectile, new Vector3(transform.position.x + 0.9f * transform.localScale.x,
         transform.position.y, transform.position.z), Quaternion.identity);
    }

    public bool inRange()
    {
        if(transform.localScale.x > 0 && transform.position.x < charCon.transform.position.x || 
        transform.localScale.x < 0 && transform.position.x > charCon.transform.position.x) return Physics2D.OverlapCircle(transform.position, sightRadius, playerLayer);
        else return false;
    }

    public int getDirectionAwayFromPlayer()
    {
        if(charCon.transform.position.x > transform.position.x) return -1;
        else return 1;
    }

    public Vector2 getVector()
    {
        if(!charCon) charCon = GameObject.FindObjectOfType<characterControl>();
        Vector3 playerPosition = charCon.transform.position;
        Vector3 enemyPosition = transform.position;
        return new Vector2(playerPosition.x - enemyPosition.x, playerPosition.y - enemyPosition.y).normalized;
    }

    public void setCanAttack()
    {
        canAttack = false;
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
}
