 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterControl: MonoBehaviour
{
    public static characterControl Instance;

    public float fallSpeed;
    [Header ("Jump Parameters")]
    public float jumpForce;
    public int jumpIndex;
    public int constDJI;
    public int addConstDJI;
    public int doubleJumpIndex;
    public float coyoteTime;
    public float coyoteTimeCounter;
    public float bufferTime;
    public float bufferTimeCounter;
    public float waitToUnlockMovement = 0.2f;
    public float wallJumpTime;
    public float wallJumpCooldown;
    public bool wallJumpWait;
    public float _wallJumpModifierX;
    public float _wallJumpModifierY;

    [Header ("Movement Parameters")]
    //public Vector2 _moveInput;
    public int direction = 1;
    public float fallSpeedBeforeCTRL;   
    public float moveSpeed;
    public float runSpeed;
    public float maxRunSpeed;
    public float crouchSpeed;
    public bool noClipOn;
    public float slideSpeed;
    public float slideAccel;
    public float speedDif;
    public float targetSpeed;
    public float movement;

    #region Acceleration Variables
    [Header ("Acceleration")]
    public float accelerationSpeed;
    public bool isAccelerating;
    public int accelerationDirection;
    #endregion

    #region Condition Checking Variables
    [Header ("Condition check")]
    public bool _isGrounded;
    public bool _isJumping;
    public bool _isMoving;
    public bool _isUnderTerrain;
    public bool _isCrouching;
    public bool _dashInAir;
    public bool _isDashing;
    public bool _isWalled;
    public bool _wasWalled;
    public float HorizontalVelocity;
    public bool movementLocked;
    public bool onLadder;
    public bool _isClimbing;
    public bool _isSliding;
    #endregion

    #region References
    [Header ("References")]
    public LayerMask Ground;
    public LayerMask Wall;
    public Rigidbody2D myrigidbody;
    public PolygonCollider2D coll;
    public PolygonCollider2D collCrouch;
    public Animator anim;
    private GameObject currentOneWayPlatform;
    // private playerHealthManager pHM;
    private cameraMovement camMov;
    private cameraFade camFade;
    #endregion

    [Header ("BoxCast Parameters")]
    public Vector3 boxSize;
    public float maxDistance;
    public Vector3 wallBoxSize;

    #region Dashing Variables
    [Header ("Dashing")]
    public float dashSpeed;
    public float dashTime;
    public float dashInvincibilityTime;
    public bool canDash = true;
    public float dashCooldownTime;
    #endregion


    #region Abilities
    [Header ("Abilities")]
    public bool hasDash;
    public bool hasDoubleJump;
    public bool hasHighJump;
    public bool hasSpecialAttack;
    public bool hasBarrier;
    public bool hasDeflectProjectile;
    public bool hasWallJump;
    public bool hasGlider;
    public bool hasTeleport;
    public bool hasHook;
    #endregion

    [Header ("Fall Attack")]
    public bool fallAttackActive;
    public double fallStartingHeight;
    public double fallEndingHeight;
    public double fallHeight;
    public bool gonnaAttack;
    public double heightToDealDamage;

    [Header ("Glider")]
    public bool isGliding;
    public bool flyUpwards;

    [Header ("Teleport")]
    [SerializeField] private float _checkCollisionRadius;
    [SerializeField] private float _findFreeSpaceRadius;
    private Vector2 _teleportPosition;
    [SerializeField] private LayerMask _collisionLayer;
    public Collider2D[] hitColliders;
    private bool _inOtherWorld;
    [SerializeField] private float _teleportTimer;
    [SerializeField] private float _teleportTimerCooldown;
    [SerializeField] private float _searchStep;
    [SerializeField] private int _maxSearchAttempts;
    private float _searchRadius;
    #region Grappling Hook Variables
    [Header ("Grappling hook")]
    public DistanceJoint2D distanceJoint;
    public Vector3 mousePos;
    public RaycastHit2D hookHit;
    public RaycastHit2D hookHitMovingTarget;
    public bool hookHitStatic;
    public bool hookHitMoving;
    public RaycastHit2D displayWhereHit;
    public float hookMaxDistance;
    public float hookCurrentDistance;
    public LayerMask hookableLayers;
    public LayerMask hookableMovingLayers;
    public Vector3 playerToMouseDirection;
    public LineRenderer lineRenderer;
    public GameObject displayHitSpriteObject;
    public SpriteRenderer displayHitSprite;
    public float hookMoveSpeed;
    public float actualHookDistance;
    #endregion

    public float LastOnWallTime;
    public float LastOnGroundTime;
    private float force;

    
    private float vertical;
    [SerializeField] private Vector2 speed;

    #region UserInput

    [field: Header ("Inputs")]    
    [field: SerializeField] public Vector2 _moveInput {get; private set;}
    public bool _jumpInput {get; private set;}
    public bool _dashInput {get; private set;}
    public bool _hookInput {get; private set;}
    public bool _crouchInput {get; private set;}
    public bool _attackInput {get; private set;}
    public bool _blockInput {get; private set;}
    public bool _switchRealmsInput {get; private set;}
    public bool _glideInput {get; private set;}
    public bool _use1Input {get; private set;}
    public bool _use2Input {get; private set;}
    public bool _menuToggleInput {get; private set;}
    public bool _activeItem1Input {get; private set;}
    #endregion
    
    //--------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------//
    //TODO: maybe redo how fall attack works, it's a bit clunky with the == 0 implementation//
    //--------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------//
    

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        // pHM = GameObject.FindObjectOfType<playerHealthManager>();
        canDash = true;
        doubleJumpIndex = constDJI + addConstDJI;
        camMov = GameObject.FindObjectOfType<cameraMovement>();
        camFade = GameObject.FindObjectOfType<cameraFade>();
        displayHitSprite = displayHitSpriteObject.GetComponent<SpriteRenderer>();
        wallJumpCooldown = wallJumpTime;
    }

    void Update()
    {
        speed = myrigidbody.velocity;
        GetInput();
        //---Teleportation to other world
        if(hasTeleport)
        {
            if(_switchRealmsInput)
            {
                distanceJoint.enabled = false;
                camMov.virtualCamera.enabled = false;
                myrigidbody.bodyType = RigidbodyType2D.Static;
                camFade.doFade();
                if(_inOtherWorld) _teleportPosition = new Vector2(transform.position.x, transform.position.y - 510);
                else _teleportPosition = new Vector2(transform.position.x, transform.position.y + 510);
                transform.position = CheckCollisionOnTeleport(_teleportPosition);
                camMov.virtualCamera.enabled = true;
                _inOtherWorld = !_inOtherWorld;
            }
            if(myrigidbody.bodyType == RigidbodyType2D.Static) _teleportTimerCooldown -= Time.deltaTime;
            if(_teleportTimerCooldown <= 0)
            {
                myrigidbody.bodyType = RigidbodyType2D.Dynamic;
                _teleportTimerCooldown = _teleportTimer;
            }
        }
        //-------------------------------

        if(hasHook) Hook();
        if(hasGlider) Glide();
        ToggleNoClip();
        //---TODO: figure out if this works or not, if i even need this 
        //---EDIT: YOLO
        // if(myrigidbody.velocity.x > 60 || myrigidbody.velocity.x < -60) 
        // {
        //     myrigidbody.velocity = new Vector2(40, myrigidbody.velocity.y);
        //     Debug.Log("Player had large horizontal velocity spike-------------------------------------------------------------------------");
        // }
        //-------------------------------------------------------------

        if(Input.GetKey(KeyCode.P)) GiveAllSkills();
        HorizontalVelocity = myrigidbody.velocity.x;
        if(isGrounded()) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= Time.deltaTime;

        //---TODO: figure out if this works or not, if i even need this
        if(_moveInput.x != 0 && !isWalled() && playerHealthManager.Instance.canMove) transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * Mathf.Sign(_moveInput.x), transform.localScale.y);
        else if(myrigidbody.velocity.x != 0) Rotate();
        //-------------------------------------------------------------

        wallMovement();
        Crouch();
        checkConditions();
        
        fallSpeed = myrigidbody.velocity.y;
        if(hasDoubleJump) 
        {
            jumpIndex = 0;
            //constDJI = 1;
        }
        if(_dashInput && canDash && hasDash && !isGliding && !isCrouching() && !isWalled())
        {
            StartCoroutine(Dash());
        }
        if(isGrounded() && _dashInAir)
        {   
            _dashInAir = false;
            canDash = true;
        }
        _isGrounded = isGrounded();
        _isMoving = isMoving();
        _isUnderTerrain = isUnderTerrain();
        _isWalled = isWalled();
        _isJumping = isJumping();
        if(movingWithoutInput()) myrigidbody.velocity = Vector3.zero; 
        vertical = Input.GetAxisRaw("Vertical");
        if(CanSlide() && Mathf.Abs(_moveInput.x) > 0 && isWalled() && _isSliding || _isDashing) myrigidbody.gravityScale = 0f;
        else if(!characterAttack.Instance._isAttacking) myrigidbody.gravityScale = 7f;
        Jump();
        LastOnWallTime -= Time.deltaTime;
        if(!isJumping()) LastOnWallTime = coyoteTime;
    }
    private void FixedUpdate()
    {
        distanceJoint.distance = hookCurrentDistance;
        hookCurrentDistance = Mathf.Clamp(hookCurrentDistance + Input.GetAxisRaw("Vertical") * hookMoveSpeed * Time.fixedDeltaTime, 0, hookMaxDistance);
        
        if(playerHealthManager.Instance.canMove) Move();
        if(_isDashing) myrigidbody.velocity = new Vector2(transform.localScale.normalized.x * (dashSpeed + Mathf.Abs(myrigidbody.velocity.x)/8), 0);
        if(_isSliding) Slide();
    }
    #region Abilities
    private void Move()
    {
        // _moveInput.x = Input.GetAxisRaw("Horizontal");
		// _moveInput.y = Input.GetAxisRaw("Vertical");
        _moveInput = UserInput.Instance.MoveInput;

        if (_isCrouching)
        {
            moveSpeed = crouchSpeed;
            isAccelerating = false;
        }
        else
        {
            moveSpeed = runSpeed;
            if (_moveInput.x != 0 && !distanceJoint.enabled)
            {
                if (Mathf.Sign(_moveInput.x) != Mathf.Sign(accelerationDirection)) isAccelerating = false;
                else isAccelerating = true;
            }
            else isAccelerating = false;
        }

        accelerationDirection = (int)_moveInput.x;
        if(isAccelerating) targetSpeed = Mathf.Clamp(Mathf.Abs(targetSpeed) + Time.fixedDeltaTime * accelerationSpeed, runSpeed, maxRunSpeed) * _moveInput.x;
        else targetSpeed = _moveInput.x * moveSpeed;

        targetSpeed = Mathf.Lerp(myrigidbody.velocity.x, targetSpeed, 1/*lerpAmount*/);
        
        if(!distanceJoint.enabled)speedDif = targetSpeed - myrigidbody.velocity.x;
        else speedDif = targetSpeed;

		movement = speedDif * 5;

        if(distanceJoint.enabled) myrigidbody.AddForce(movement / 3.2f * Vector2.right, ForceMode2D.Force);
        else myrigidbody.AddForce(movement * Vector2.right, ForceMode2D.Force);

        if(isGrounded() && _moveInput.x != 0) anim.SetBool("isMoving", true);
        else anim.SetBool("isMoving", false);
    }
    private void Jump()
    {
        force = jumpForce;
        if (myrigidbody.velocity.y < 0)
            force -= myrigidbody.velocity.y;
    
        if(UserInput.Instance._jumpAction.WasPressedThisFrame() && !isGrounded()) 
            bufferTimeCounter = bufferTime;

        if(bufferTimeCounter > 0 && isGrounded()) 
        {
            if (myrigidbody.velocity.y > 0)
                force -= myrigidbody.velocity.y;

            myrigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }
        if(bufferTimeCounter > 0) bufferTimeCounter -= Time.deltaTime;
        
        if ((UserInput.Instance._jumpAction.WasPressedThisFrame() && isGrounded() 
        || UserInput.Instance._jumpAction.WasPressedThisFrame() && doubleJumpIndex > 0 
        || UserInput.Instance._jumpAction.WasPressedThisFrame() && coyoteTimeCounter > 0) 
        && !_isSliding && playerHealthManager.Instance.canMove)
        {
            if (myrigidbody.velocity.y > 0)
                force -= myrigidbody.velocity.y;

            myrigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);

            if(!isGrounded() && coyoteTimeCounter < 0 && !distanceJoint.enabled) 
                --doubleJumpIndex;
        }
        else if(UserInput.Instance._jumpAction.WasPressedThisFrame() && isWalled() && _isSliding && Mathf.Abs(_moveInput.x) > 0) // wall jump 
        {
            //isWallJumping = true;
            //Vector2 wallJumpForce = new Vector2(jumpForce/3 * -Mathf.Sign(transform.localScale.x)/* * 30*/, jumpForce * 1.2f /** 50*/);
            //Vector2 wallJumpForce = new Vector2(jumpForce/3 * -Mathf.Sign(transform.localScale.x)/* * 30*/ * 1f, jumpForce * 1.2f/* * 50*/);
            Vector2 wallJumpForce = new Vector2(jumpForce * _wallJumpModifierX * -Mathf.Sign(transform.localScale.x), jumpForce * _wallJumpModifierY);
            //Vector2 wallJumpForce = new Vector2(0, jumpForce * 2);
            playerHealthManager.Instance.canMove = false;

            //if (Mathf.Sign(myrigidbody.velocity.x) != Mathf.Sign(wallJumpForce.x))
            //    wallJumpForce.x -= myrigidbody.velocity.x;

            if (myrigidbody.velocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
                wallJumpForce.y -= myrigidbody.velocity.y;
            //myrigidbody.velocity = new Vector2(myrigidbody.velocity.x, 0);
            //myrigidbody.velocity = Vector2.zero;
            //myrigidbody.AddForce(wallJumpForce + new Vector2(0, Time.deltaTime * 320), ForceMode2D.Impulse);
            myrigidbody.AddForce(wallJumpForce, ForceMode2D.Impulse);
            //myrigidbody.AddForce(wallJumpForce, ForceMode2D.Force);
            //myrigidbody.velocity = new Vector2(8 * -Mathf.Sign(transform.localScale.x), 32);
            //myrigidbody.velocity = new Vector2(wallJumpForce.x, wallJumpForce.y + Time.deltaTime * 400);
            movementLocked = true;
            wallJumpWait = true;
            //playerHealthManager.Instance.canMove = true;
        }

        if(UserInput.Instance._jumpAction.WasPressedThisFrame()) coyoteTimeCounter = 0;

        if ((isGrounded() || isWalled() && _isSliding) && !distanceJoint.enabled)
        {
            doubleJumpIndex = constDJI + addConstDJI;
        }

        //---Variable height part of the jump
        
        if (UserInput.Instance._jumpAction.WasPressedThisFrame() && isGrounded() && !_isSliding)
            myrigidbody.velocity = new Vector2(myrigidbody.velocity.x, jumpForce);

        if (UserInput.Instance._jumpAction.WasReleasedThisFrame() && myrigidbody.velocity.y > 0 && doubleJumpIndex == constDJI && !movementLocked && !_isSliding)
            myrigidbody.velocity = new Vector2(myrigidbody.velocity.x, myrigidbody.velocity.y / 4);
            
        //-----------------------------------

        if(movementLocked)
        {
            waitToUnlockMovement -= Time.deltaTime;
            if(waitToUnlockMovement < 0) 
            {
                playerHealthManager.Instance.canMove = true;
                waitToUnlockMovement = 0.3f;
                movementLocked = false;
            }
        }

        if(wallJumpWait)
        {
            wallJumpCooldown -= Time.deltaTime;
            if(wallJumpCooldown < 0) 
            {
                playerHealthManager.Instance.canMove = true;
                wallJumpCooldown = wallJumpTime;
                wallJumpWait = false;
            }
        }
    } 
    private void wallMovement()
    {
        if(isWalled() && Mathf.Abs(_moveInput.x) > 0 && CanSlide()/* && !distanceJoint.enabled*/) _isSliding = true;
        else _isSliding = false;
    }
    private void Crouch()
    {
        if(UserInput.Instance._crouchAction.IsPressed() && isGrounded() || _isCrouching)
        {
            anim.SetBool("isCrouching", true);
            collCrouch.enabled = true;
            coll.enabled = false;

        }
        else if(UserInput.Instance._crouchAction.IsPressed())
        {
            // ------- fall attack item
            //
            //  fallAttackActive = true;
            //  fallStartingHeight = transform.position.y;
            //  if(fallSpeedBeforeCTRL == 0) fallSpeedBeforeCTRL = myrigidbody.velocity.y;
            //
            // -------
            //
            //myrigidbody.velocity = new Vector2(myrigidbody.velocity.x, -15);
            myrigidbody.AddForce(new Vector2(0, -15));
        }
        // if(UserInput.Instance._crouchAction.WasReleasedThisFrame() && !isGrounded())
        // {
        //     fallAttackActive = false;
        //     myrigidbody.velocity = new Vector2(myrigidbody.velocity.x, fall SpeedBeforeCTRL);
        // }
        if(!UserInput.Instance._crouchAction.IsPressed() && !isUnderTerrain() || isJumping())
        {
            anim.SetBool("isCrouching", false);
            collCrouch.enabled = false;
            coll.enabled = true;
        }
        //
        // ------- fall attack item
        //
        // if(isGrounded() && fallAttackActive)
        // {
        //     fallSpeedBeforeCTRL = 0;
        //     fallEndingHeight = transform.position.y;
        //     fallHeight = fallStartingHeight - fallEndingHeight;
        // }
        // else fallHeight = 0;
        // if(fallHeight >= heightToDealDamage && isGrounded() && isCrouching())
        // {
        //     fastFallAttack();
        //     gonnaAttack = true;
        // }
        // else gonnaAttack = false;
        //
        // -------
        //
    }
    private bool movingWithoutInput()
    {
        if(isMoving() && Input.GetAxis("Horizontal") == 0) return true;
        else return false;
    }
    public void Rotate()
    {
        if (myrigidbody.velocity.x > 0.05)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (myrigidbody.velocity.x < -0.05)
        {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    public void fastFallAttack()
    {
        fallHeight = 0;
        fallAttackActive = false;
        characterAttack.Instance.fastFallAttack = true;
        StartCoroutine(characterAttack.Instance.hitEnemies());
    }  
    public void Hook()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerToMouseDirection = (mousePos - transform.position).normalized;        
        if(_hookInput && !_isCrouching)
        {
            distanceJoint.enabled = false;
            hookHit = Physics2D.Raycast(transform.position/* - Vector3.up * 0.5f*/, playerToMouseDirection, hookMaxDistance, hookableLayers);
            hookHitMovingTarget = Physics2D.Raycast(transform.position, playerToMouseDirection, hookMaxDistance, hookableMovingLayers);
            if(hookHitMovingTarget.collider != null)
            {  
                Debug.Log("Hooked to moving target"); 
                hookCurrentDistance = hookHit.distance;
                distanceJoint.enabled = true;
                distanceJoint.connectedBody = hookHit.rigidbody;
                distanceJoint.connectedAnchor = Vector2.zero;
                doubleJumpIndex = constDJI + 1 + addConstDJI;
            }
            else if(hookHit.collider != null)
            {   
                Debug.Log("Hooked to static target"); 
                hookCurrentDistance = hookHit.distance;
                distanceJoint.enabled = true;
                distanceJoint.connectedBody = null;
                distanceJoint.connectedAnchor = hookHit.point;
                doubleJumpIndex = constDJI + 1 + addConstDJI;
            }
        }
        if((_jumpInput || _isDashing || Input.GetKeyDown(KeyCode.LeftControl) || playerHealthManager.Instance.playerDead) && distanceJoint.enabled) distanceJoint.enabled = false;
        if(distanceJoint.enabled) 
        {
            lineRenderer.SetPosition(0, transform.position);
            if(hookHitMovingTarget.collider != null) 
            {
                lineRenderer.SetPosition(1, hookHit.rigidbody.transform.position);
                if(hookCurrentDistance >= 6.95f) actualHookDistance = Vector2.Distance(transform.position, hookHit.rigidbody.transform.position);
                else actualHookDistance = hookCurrentDistance;
            }
            else if(hookHit.collider != null) 
            {
                lineRenderer.SetPosition(1, hookHit.point);
                if(hookCurrentDistance >= 6.95f) actualHookDistance = Vector2.Distance(transform.position, hookHit.point);
                else actualHookDistance = hookCurrentDistance;
            }
            if(actualHookDistance >= 7.1f) distanceJoint.enabled = false;
        }
        else 
        {
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);
        }

        displayWhereHit = Physics2D.Raycast(transform.position, playerToMouseDirection, hookMaxDistance * 100, hookableLayers);
        if(displayWhereHit.distance > hookMaxDistance || _isCrouching) displayHitSprite.color = Color.red;
        else displayHitSprite.color = Color.white;
        if(displayWhereHit.collider != null)
        {   
            displayHitSpriteObject.transform.position = displayWhereHit.point;
        }
    }

    public void Slide()
    {
        float speedDif = slideSpeed - myrigidbody.velocity.y;	
		float movement = speedDif * slideAccel;
		movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif)  * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));
		if(playerHealthManager.Instance.canMove) myrigidbody.AddForce(movement * Vector2.up);
    }
    public void Glide()
    {
        if(UserInput.Instance._glideAction.IsPressed() && !distanceJoint.enabled)
        {
            if(flyUpwards) myrigidbody.velocity = new Vector2(myrigidbody.velocity.x, 5.5f);
            else myrigidbody.velocity = new Vector2(myrigidbody.velocity.x, -3.5f);
        }
    }
    public Vector3 CheckCollisionOnTeleport(Vector3 TeleportPosition)
    {
        Debug.Log("Check collision on teleport");
        FindFreeSpaceOnTeleport(TeleportPosition);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(TeleportPosition, _checkCollisionRadius, _collisionLayer);
        hitColliders = colliders;
        if(colliders.Length > 0) TeleportPosition = FindFreeSpaceOnTeleport(TeleportPosition);
        return TeleportPosition;
    }

    public Vector3 FindFreeSpaceOnTeleport(Vector3 TeleportPosition)
    {
        Debug.Log("Finding free space for teleport");
        _searchRadius = _findFreeSpaceRadius;

        for (int i = 0; i < _maxSearchAttempts; i++)
        {
            _searchRadius += _searchStep;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(TeleportPosition, _searchRadius, _collisionLayer);
            if (colliders.Length == 0)
            {
                return TeleportPosition;
            }

            TeleportPosition += Random.insideUnitSphere * _searchStep;
        }
        return TeleportPosition;
    }
    
    #endregion
    #region Conditions
    private bool isJumping()
    {
        if(!isUnderTerrain())
        {
            if (myrigidbody.velocity.y > 0.1 && _jumpInput) return true;
            else if (!isGrounded() && !isFalling()) return true;
            else return false;
        }
        else return false;
    }
    private bool isFalling()
    {
        if (myrigidbody.velocity.y < -0.1 && !isGrounded()) return true;
        return false;   
    }
    public bool isGrounded()
    {
        if((Physics2D.BoxCast(coll.bounds.center, boxSize + new Vector3(1f,0,0), 0f, -transform.up, maxDistance, Ground) || 
        Physics2D.BoxCast(collCrouch.bounds.center, boxSize, 0f, -transform.up, maxDistance, Ground))/* && !distanceJoint.enabled*/) return true;
        else return false;
    }
    public bool isWalled()
    {
        if(Physics2D.BoxCast(coll.bounds.center, wallBoxSize, 0f, transform.right * Mathf.Sign(transform.localScale.x), maxDistance, Wall) && hasWallJump) return true;
        else return false;
    }
    public bool isUnderTerrain()
    {
        return Physics2D.OverlapBox(collCrouch.bounds.center + Vector3.up * 0.75f, boxSize, 0, Ground);
    }
    public bool isMoving()
    {
        if(myrigidbody.velocity.x != 0 && isGrounded()) return true;
        else return false;
    }
    public bool isCrouching()
    {
        if(UserInput.Instance._crouchAction.IsPressed() && isGrounded() || isUnderTerrain() && collCrouch.enabled == true) _isCrouching = true;
        else _isCrouching = false;
        return _isCrouching;
    }
    public bool CanSlide()
    {
		if (LastOnWallTime > 0 && /*!IsJumping && !IsWallJumping &&*/ LastOnGroundTime <= 0) return true;
		else return false;
	}

    private void checkConditions()
    {
        //if(isMoving()) anim.SetBool("isMoving", true);
        //else anim.SetBool("isMoving", false);

        if(isJumping()) anim.SetBool("isJumping", true);
        else anim.SetBool("isJumping", false);

        if(isFalling()) anim.SetBool("isFalling", true);
        else anim.SetBool("isFalling", false);

        if(isGrounded()) anim.SetBool("isGrounded", true);
        else anim.SetBool("isGrounded", false);

        isCrouching();
    }
    #endregion
    #region Coroutines
    IEnumerator Dash()
    {
        _isDashing = true;
        if(!isGrounded()) _dashInAir = true;
        canDash = false;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("player"), LayerMask.NameToLayer("enemy"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("player"), LayerMask.NameToLayer("enemyProjectile"), true);
        yield return new WaitForSeconds(dashTime);
        _isDashing = false;
        //TODO: figure out which is better to stop player after a dash or not
        //myrigidbody.velocity = Vector2.zero;
        yield return new WaitForSeconds(dashInvincibilityTime - dashTime);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("player"), LayerMask.NameToLayer("enemyProjectile"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("player"), LayerMask.NameToLayer("enemy"), false);
        yield return new WaitForSeconds(dashCooldownTime - (dashInvincibilityTime - dashTime));
        canDash = true;
    }    

    IEnumerator CheckWallJump()
    {
        yield return new WaitForSeconds(0.3f);
        if(doubleJumpIndex != constDJI && !distanceJoint.enabled) doubleJumpIndex = constDJI + addConstDJI;
    }
    #endregion
    
    #region Triggers
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "UpwardsWind")
        {
            flyUpwards = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "UpwardsWind")
        {
            flyUpwards = false;
        }
    }
    #endregion
    
    #region Gizmos

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(coll.bounds.center - transform.up * maxDistance, boxSize + new Vector3(1f,0,0));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(coll.bounds.center - transform.right * maxDistance * Mathf.Sign(transform.localScale.x) * -1, wallBoxSize);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _checkCollisionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _findFreeSpaceRadius);
    }
    #endregion
    void GetInput()
    {
        _moveInput = UserInput.Instance.MoveInput;
        _jumpInput = UserInput.Instance.JumpInput;
        _dashInput = UserInput.Instance.DashInput;
        _hookInput = UserInput.Instance.HookInput;
        _crouchInput = UserInput.Instance.CrouchInput;
        _attackInput = UserInput.Instance.AttackInput;
        _blockInput = UserInput.Instance.BlockInput;
        _switchRealmsInput = UserInput.Instance.SwitchRealmsInput;
        _glideInput = UserInput.Instance.GlideInput;
        _use1Input = UserInput.Instance.Use1Input;
        _use2Input = UserInput.Instance.Use2Input;
        _menuToggleInput = UserInput.Instance.MenuToggleInput; 
        _activeItem1Input = UserInput.Instance.ActiveItem1Input;
    }

    #region Testing Functionality
    private void GiveAllSkills()
    {
        hasDash = true;
        hasDoubleJump = true;
        hasHighJump = true;
        hasSpecialAttack = true;
        hasBarrier = true;
        hasDeflectProjectile = true;
        hasWallJump = true;
        hasGlider = true;
        hasTeleport = true;
    }

    void ToggleNoClip()
    {   
        if(Input.GetKeyDown(KeyCode.BackQuote))
        {
            if(noClipOn) EnableCollision();
            else DisableCollision();
        }

        if(Input.GetKey(KeyCode.UpArrow)) myrigidbody.velocity = new Vector2(myrigidbody.velocity.x, moveSpeed * 1);
        if(Input.GetKey(KeyCode.DownArrow)) myrigidbody.velocity = new Vector2(myrigidbody.velocity.x, moveSpeed * -1);
        if(Input.GetKey(KeyCode.RightArrow)) myrigidbody.velocity = new Vector2(moveSpeed * 1, myrigidbody.velocity.y);
        if(Input.GetKey(KeyCode.LeftArrow)) myrigidbody.velocity = new Vector2(moveSpeed * -1, myrigidbody.velocity.y);
        if(Input.GetKeyUp(KeyCode.UpArrow)) myrigidbody.velocity = new Vector2(myrigidbody.velocity.x, 0);
        if(Input.GetKeyUp(KeyCode.DownArrow)) myrigidbody.velocity = new Vector2(myrigidbody.velocity.x, 0);
        if(Input.GetKeyUp(KeyCode.RightArrow)) myrigidbody.velocity = new Vector2(0, myrigidbody.velocity.y);
        if(Input.GetKeyUp(KeyCode.LeftArrow)) myrigidbody.velocity = new Vector2(0, myrigidbody.velocity.y);
    }

    void DisableCollision()
    {
        myrigidbody.isKinematic = true;
        noClipOn = true;
    }

    void EnableCollision()
    {
        myrigidbody.isKinematic = false;
        noClipOn = false;
    }
    #endregion
}

