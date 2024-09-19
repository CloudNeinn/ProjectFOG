using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NightCrawlerBoss : MonoBehaviour, IAttackable, IJumpableChase, IChaseable, IRadSeeable, IParryable, IVariedAttacks, IWalkable
{
    [SerializeField] private Vector3 _playerPosition;
    [SerializeField] private Vector3 _enemyPosition;
    [SerializeField] private Rigidbody2D _enemyrb;
    [SerializeField] private CircleCollider2D _enemycol;

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
    
    [field: Header ("Movement Options")]
    [field: SerializeField] public float moveSpeed { get; set; }
    [field: SerializeField] public int directionX { get; set; }
    [field: SerializeField] public bool isStanding { get; set; }

    [field: Header ("Pathfinding")]

    [field: SerializeField] public Seeker seeker { get; set; }
    [field: SerializeField] public Path path { get; set; }
    [field: SerializeField] public int currentWaypoint { get; set; } 
    [field: SerializeField] public Vector3 target { get; set; } 

    [field: SerializeField] public Transform playerPosition { get; set; } 
    [field: SerializeField] public Vector3 enemyStartingPosition { get; set; } 
    [field: SerializeField] public float activateDistance { get; set; } //50f
    [field: SerializeField] public float pathUpdateSeconds { get; set; } //0.5f
 
    [field: SerializeField] public bool followEnabled { get; set; } 
    [field: SerializeField] public bool jumpEnabled { get; set; } 
    [field: SerializeField] public float jumpModifier { get; set; } // 0.3f
    [field: SerializeField] public float jumpNodeHeightRequirement { get; set; } //0.8f
    [field: SerializeField] public float nextWaypointDistance { get; set; } //3f
    [field: SerializeField] public float speed { get; set; } //200f

    [field: SerializeField] public float jumpCooldown { get; set; }
    [field: SerializeField] public float jumpTimer { get; set; }
    [field: SerializeField] public Vector2 force { get; set; }
    [field: SerializeField] public Vector2 direction { get; set; }

    [field: Header ("Different attack range options")]
    [field: Header ("Long attack")]
    [field: SerializeField] public Vector2 longAttackRangeBox { get; set; }
    [field: SerializeField] public Vector2 longAttackRange { get; set; }
    [field: Header ("Medium attack")]
    [field: SerializeField] public Vector2 mediumAttackRangeBox { get; set; }
    [field: SerializeField] public Vector2 mediumAttackRange { get; set; }
    [field: Header ("Close attack")]
    [field: SerializeField] public Vector2 closeAttackRangeBox { get; set; }
    [field: SerializeField] public Vector2 closeAttackRange { get; set; }

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

    [field: Header ("Additional Movement Options")]
    [field: SerializeField] public float patrolSpeed { get; set; }
    [field: SerializeField] public float attackSpeed { get; set; }
    [field: SerializeField] public float noticeStandingTime { get; set; }
    [field: SerializeField] public float noticeStandingCooldown { get; set; }

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

    [field: Header ("Parrying")]
    [field: SerializeField] public bool isParrying { get; set;}
    [field: SerializeField] public float parryTime { get; set;}
    public Vector2 aboveBoxSize;
    public float aboveBoxOffset;
    [SerializeField] private bool _teleportationAttack;
    [SerializeField] private float _teleportationAttackTime;
    [SerializeField] private float _teleportationAttackCooldown;

    void Start()
    {  
        playerPosition = characterControl.Instance.transform;
        enemyStartingPosition = transform.position;
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    void Update()
    {
        Rotate();
        if(inSight())
        {
            isAlert = true;
            EventManager.CloseDoor(GetComponent<enemyOpenDoor>()._doorID);
        }

        if(isAlert) target = playerPosition.position;

        if(inCloseAttackRange() && !_teleportationAttack)
        {
            Stand();
            followEnabled = false;
        }
        else if(isAlert && !_teleportationAttack) followEnabled = true; 

        if(followEnabled) PathFollow();

        if(!inRange() && isAlert) 
        {
            followEnabled = false;
            _teleportationAttack = true;
            transform.position = new Vector2(characterControl.Instance.transform.position.x + Mathf.Sign(characterControl.Instance.transform.localScale.x) * -2f, characterControl.Instance.transform.position.y + 3f);
            transform.localScale = new Vector2(Mathf.Sign(characterControl.Instance.transform.localScale.x) * Mathf.Abs(transform.localScale.y), transform.localScale.y);
            _enemyrb.bodyType = RigidbodyType2D.Static;
        } 
        
        if(_teleportationAttack) teleportAttack();
    }

    void FixedUpdate()
    {
        
    }

    public void Attack()
    {

    }

    public void teleportAttack()
    {
        //
        if(_teleportationAttackCooldown > 0) _teleportationAttackCooldown -= Time.deltaTime;
        else if(_enemyrb.bodyType == RigidbodyType2D.Static)
        {
            _enemyrb.bodyType = RigidbodyType2D.Kinematic;
            _enemyrb.velocity = getVector() * moveSpeed * 6;
        }

        if(isGrounded() && _enemyrb.bodyType == RigidbodyType2D.Kinematic) 
        {
            _enemyrb.bodyType = RigidbodyType2D.Dynamic;
            _teleportationAttackCooldown = _teleportationAttackTime;
            _teleportationAttack = false;
            followEnabled = true;
        }
    }

    public bool inLongAttackRange()
    {
        return Physics2D.OverlapBox(transform.position + (Vector3)longAttackRange * Mathf.Sign(transform.localScale.x), longAttackRangeBox, 0, playerLayer);
    }

    public bool inMediumAttackRange()
    {
        return Physics2D.OverlapBox(transform.position + (Vector3)mediumAttackRange * Mathf.Sign(transform.localScale.x), mediumAttackRangeBox, 0, playerLayer);
    }

    public bool inCloseAttackRange()
    {
        return Physics2D.OverlapBox(transform.position + (Vector3)closeAttackRange * Mathf.Sign(transform.localScale.x), closeAttackRangeBox, 0, playerLayer);
    }

    public void longAttack()
    {

    }

    public void mediumAttack()
    {

    }

    public void closeAttack()
    {

    }

    public Vector2 getVector()
    {
        if(!characterControl.Instance) characterControl.Instance = GameObject.FindObjectOfType<characterControl>();
        _playerPosition = characterControl.Instance.transform.position;
        _enemyPosition = transform.position;
        return new Vector2(_playerPosition.x - _enemyPosition.x, _playerPosition.y - _enemyPosition.y).normalized;
    }

    public void Block()
    {
        moveSpeed = blockingSpeed;
        blockHit = inBlockingRange();
        if(blockHit && characterControl.Instance.transform.position.x * Mathf.Sign(transform.localScale.x) >= 
            (transform.position.x + blockingBoxOffset.x * Mathf.Sign(transform.localScale.x)) * Mathf.Sign(transform.localScale.x)) eneHeaBlo.isBlocked = true;
    }

    public bool inBlockingRange()
    {
        return Physics2D.OverlapBox(transform.position + blockingBoxOffset * Mathf.Sign(transform.localScale.x), blockingBoxSize, 0, blockingLayers);
    }

    public void Parry()
    {
        Block();
        playerHealthManager.Instance.getDamage(damage, 200, 200);
    }

    public void Walk(int direction)
    {
        _enemyrb.AddForce(Vector2.right * moveSpeed * 500 * direction);
    }

    public void Stand() 
    {
        _enemyrb.velocity = new Vector2(0, _enemyrb.velocity.y);
    } 

    public void Rotate()
    {
        if (_enemyrb.velocity.x >= 0.25f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (_enemyrb.velocity.x < -0.25f)
        {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    public void ChangeDirection(float direction)
    {
        direction = direction / Mathf.Abs(direction);
        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y);
    }

    public bool checkIfPlayerAbove()
    {
        return Physics2D.OverlapBox(new Vector2(_enemycol.bounds.center.x, _enemycol.bounds.center.y - aboveBoxOffset), aboveBoxSize, 0, playerLayer);
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
        return Physics2D.OverlapBox(new Vector2(_enemycol.bounds.center.x, _enemycol.bounds.center.y - isGroundedDistance), isGroundedBox, 0, checkGroundLayer);
    }   

    public void PathFollow() 
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }  

        direction = ((Vector2)path.vectorPath[currentWaypoint] - (_enemyrb.position - Vector2.up * 0.25f)).normalized;
        force = direction * (moveSpeed * 500);

        if (jumpEnabled && isGrounded() && target.y > transform.position.y 
         && direction.y > jumpNodeHeightRequirement && !checkIfPlayerAbove() && characterControl.Instance.isGrounded()) Jump(getJumpModifier());

        if(!jumpEnabled)
        {
            jumpTimer -= Time.deltaTime;
            if(jumpTimer <= 0) jumpEnabled = true;
        }

        if(isGrounded()) _enemyrb.AddForce(force);
        else _enemyrb.AddForce(new Vector2(force.x, 0));
        //force is inconsistent and with some enemies its OK with others its not
        //_enemyrb.velocity = new Vector2(moveSpeed * Mathf.Sign(transform.localScale.x), 0);

        float distance = Vector2.Distance(_enemyrb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }    
    }

    public void UpdatePath() 
    {
        if(seeker.IsDone() && target != null) seeker.StartPath(_enemyrb.position, target, OnPathComplete);
    }

    public void OnPathComplete(Path p) 
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public float getJumpModifier() 
    {
        if(target.y > transform.position.y) jumpModifier = Mathf.Sqrt((target.y - transform.position.y)*0.3f);
        return jumpModifier * 0.9f;
    }

    public void Jump(float jumpStrength) 
    {
        _enemyrb.AddForce(Vector2.up * speed * jumpStrength * 1.2f);
        jumpEnabled = false;
        jumpTimer = jumpCooldown;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, sightRadius); 
        Gizmos.DrawWireCube(_enemycol.bounds.center - transform.up * isGroundedDistance, isGroundedBox);  
        Gizmos.DrawWireCube(_enemycol.bounds.center - transform.up * aboveBoxOffset, aboveBoxSize);  

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_enemycol.bounds.center + (Vector3)longAttackRange * Mathf.Sign(transform.localScale.x), longAttackRangeBox); 

        Gizmos.color = Color.yellow; 
        Gizmos.DrawWireCube(_enemycol.bounds.center + (Vector3)mediumAttackRange * Mathf.Sign(transform.localScale.x), mediumAttackRangeBox); 

        Gizmos.color = Color.green; 
        Gizmos.DrawWireCube(_enemycol.bounds.center + (Vector3)closeAttackRange * Mathf.Sign(transform.localScale.x), closeAttackRangeBox);  
    }
}