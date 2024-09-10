using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CorruptFrog : Frog, IRadSeeable, IAttackable, IChaseable, IJumpableChase
{    
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
    [field: SerializeField] public float jumpNodeHeightRequirement { get; set; } //0.8f
    [field: SerializeField] public float nextWaypointDistance { get; set; } //3f
    [field: SerializeField] public float speed { get; set; } //200f
    [field: SerializeField] public Vector2 force { get; set; }
    [field: SerializeField] public Vector2 direction { get; set; }


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
    [field: SerializeField] public float empoweredJumpModifier { get; set; }
    [field: SerializeField] public bool isEmpoweredJump { get; set; }
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
    [field: SerializeField] public bool isAlert { get; set; }
    [field: SerializeField] public bool isSensing { get; set; }
    [field: SerializeField] public Vector2 lastPlayerPos { get; set; }

    [field: Header ("Layer Masks")]
    [field: SerializeField] public LayerMask playerLayer { get; set; }
    [field: SerializeField] public LayerMask raycastLayer { get; set; }
    [field: SerializeField] public LayerMask checkWallLayer { get; set; }

    [field: Header ("Different attack range options")]
    [field: SerializeField] public float LongRangeRadius { get; set; }
    [field: SerializeField] public float MediumRangeRadius { get; set; }
    [field: SerializeField] public float ShortRangeRadius { get; set; }

    
    void Start()
    {
        playerPosition = characterControl.Instance.transform;
        enemyStartingPosition = transform.position;
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }
    
    void Update()
    {
        //if(isStanding) Stand();
        if(followEnabled && !isStanding) PathFollow();
        if(jumpTimer > 0 && !jumpEnabled && !isStanding) jumpTimer -= Time.deltaTime;
        else if(!isStanding) jumpEnabled = true;
        if(inSight() && Vector2.Distance(transform.position, enemyStartingPosition) <= 15f) 
        {
            isAlert = true;
            forgetCooldown = forgetTime;
        }
        else if(forgetCooldown <= 0 || Vector2.Distance(transform.position, enemyStartingPosition) > 15f)
        {
            isAlert = false;
            target = enemyStartingPosition;
        }
        if(isAlert && followEnabled && Vector2.Distance(transform.position, enemyStartingPosition) <= 5f) followEnabled = false;
        if(isAlert && !inSight() && forgetCooldown > 0) forgetCooldown -= Time.deltaTime;
        if(isAlert) {
            followEnabled = true;
            target = playerPosition.position;
        }
        Rotate();
        if(isAlert && attackCooldown <= 0) canAttack = true;
        else if(isAlert && attackCooldown > 0) 
        {
            attackCooldown -= Time.deltaTime;
            //isStanding = false;
        }
        else attackCooldown = attackTime;
        if(canAttack) Attack();
        if(inShortRange() && isAlert && isGrounded())
        {
            Stand();
            isStanding = true;
        }
        else if(!canAttack && isAlert && isGrounded()) isStanding = false;
    }
    void FixedUpdate()
    {
        if(isPatroling && canPatrol && !isAlert) Patrol();

    }
    public void Attack()
    {
        isStanding = true;
        Stand();
        if(inShortRange() && isAlert) 
        {
            //tongue attack
            Debug.Log("Short");
        }
        else if(inMediumRange() && isAlert && isGrounded() && jumpEnabled && inSight())
        {
            attackCooldown = attackTime;
            _enemyrb.AddForce(new Vector2(getVector().x + Mathf.Sign(getVector().x) * jumpHeight * 6, jumpHeight * 3) * (moveSpeed * 50));
            Debug.Log("Middle");
        }
        else if(inLongRange() && isGrounded() && isAlert || inMediumRange() && isAlert && !inSight())
        {
            //spawn projectile
            Debug.Log("Long");
            //after attack animation is done isStanding = false;
        }
        canAttack = false;
    }

    public bool inLongRange()
    {
        return Physics2D.OverlapCircle(transform.position, LongRangeRadius, playerLayer);
    }

    public bool inMediumRange()
    {
        return Physics2D.OverlapCircle(transform.position, MediumRangeRadius, playerLayer);
    }

    public bool inShortRange()
    {
        return Physics2D.OverlapCircle(transform.position, ShortRangeRadius, playerLayer);
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
        return Physics2D.OverlapBox(new Vector2(_enemycol.bounds.center.x + checkGroundDistance * -transform.localScale.x,
         _enemycol.bounds.center.y), checkGroundBoxSize, 0, checkGroundLayer);
    }

    public bool checkIfWall()
    {
        return Physics2D.OverlapBox(new Vector2(_enemycol.bounds.center.x + checkWallDistance * -transform.localScale.x,
         _enemycol.bounds.center.y), checkWallBoxSize, 0, checkWallLayer);
    } 
    public Vector2 getVector()
    {
        if(!characterControl.Instance) characterControl.Instance = GameObject.FindObjectOfType<characterControl>();
        _playerPosition = characterControl.Instance.transform.position;
        _enemyPosition = transform.position;
        return new Vector2(_playerPosition.x - _enemyPosition.x, _playerPosition.y - _enemyPosition.y).normalized;
    }

    /*
    should only follow player in specific radius away from its (enemy's) spawn point and also until forgets player
    also it has 3 attacks - 2 melee and ranged
    if player is far enough it frog spits a projectile that explodes on impact or after a certain time flying
    if player is close enough frog either jumps into the player or hits him with its tongue 
    (2 ways to implement either a chance of happening or a 3 distance checkers (far, middle, close) instead of 2 (close, far))
    */
    public void PathFollow() 
    {
        if (path == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }  

        direction = ((Vector2)path.vectorPath[currentWaypoint] - _enemyrb.position).normalized;
        //it is done so the frog would always jump when moving
        if(direction.y < -1.5f) direction = new Vector2(direction.x + Mathf.Sign(direction.x) * jumpHeight * 6, jumpHeight * 5);
        else if(direction.y <= 2.5f) direction = new Vector2(direction.x + Mathf.Sign(direction.x) * jumpHeight * 6, jumpHeight * 10);         
        force = direction * (moveSpeed * 50);

        // Movement
        // it should move (apply force or velocity) only when it is grounded and timer <= 0
        if(isGrounded() && jumpEnabled) {
            _enemyrb.AddForce(force);
            jumpEnabled = false;
            jumpTimer = jumpCooldown;
        }

        // Next Waypoint
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
}
