 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterControl: MonoBehaviour, IDataPersistance
{
    public float fallSpeed;
    [Header ("Jump Parameters")]
    public float jumpForce;
    public int jumpIndex;
    public int constDJI;
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
    public float crouchSpeed;
    public bool noClipOn;
    public float slideSpeed;
    public float slideAccel;
    public float speedDif;
    public float targetSpeed;
    public float movement;
    
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
    private playerHealthManager pHM;
    private cameraMovement camMov;
    private cameraFade camFade;
    public checkpointManagement checkpManage;
    private charachterAttack charAtt;
    private charachterBlock charBlo;
    #endregion

    [Header ("BoxCast Parameters")]
    public Vector3 boxSize;
    public float maxDistance;
    public Vector3 wallBoxSize;

    #region Dashing Variables
    [Header ("Dashing")]
    public float dashSpeed;
    public float dashTime;
    public bool canDash = true;
    public float dashCooldownTime;
    #endregion

    #region Acceleration Variables
    [Header ("Acceleration")]
    public float timeUntilAcceleration;
    public float accelerationSpeed;
    public float speedWhileAccelerating;
    public bool isAccelerating;
    public bool startAcceleration; 
    public float ladderSpeed;
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
    #endregion

    [Header ("Fall Atack")]
    public bool fallAttackActive;
    public double fallStartingHeight;
    public double fallEndingHeight;
    public double fallHeight;
    public bool gonnaAttack;
    public double heightToDealDamage;

    [Header ("Glider")]
    public bool isGliding;
    
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

    private bool inOtherWorld;
    public float teleportTimer;
    public float teleportTimerCooldown;
    
    private float vertical;

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
    #endregion
    
    //--------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------//
    //TODO: maybe redo how fall attack works, it's a bit clunky with the == 0 implementation//
    //--------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------//
    
    #region Load/Save system
    public void LoadData(GameData data)
    {
        
    }

    public void SaveData(ref GameData data)
    {

    }
    #endregion


    void Start()
    {
        pHM = GameObject.FindObjectOfType<playerHealthManager>();
        charAtt = gameObject.GetComponentInChildren<charachterAttack>();
        charBlo = gameObject.GetComponentInChildren<charachterBlock>();
        accelerationSpeed = runSpeed * 1.6f;
        canDash = true;
        doubleJumpIndex = constDJI;
        speedWhileAccelerating = runSpeed;
        camMov = GameObject.FindObjectOfType<cameraMovement>();
        camFade = GameObject.FindObjectOfType<cameraFade>();
        checkpManage = GameObject.FindObjectOfType<checkpointManagement>();
        displayHitSprite = displayHitSpriteObject.GetComponent<SpriteRenderer>();
        wallJumpCooldown = wallJumpTime;


    }

    void Update()
    {
        GetInput();
        //---Teleportation to other world
        if(_switchRealmsInput)
        {
            distanceJoint.enabled = false;
            camMov.virtualCamera.enabled = false;
            myrigidbody.bodyType = RigidbodyType2D.Static;
            camFade.doFade();
            if(inOtherWorld) transform.position = new Vector2(transform.position.x, transform.position.y - 160);
            else transform.position = new Vector2(transform.position.x, transform.position.y + 160);
            camMov.virtualCamera.enabled = true;
            inOtherWorld = !inOtherWorld;
        }
        if(myrigidbody.bodyType == RigidbodyType2D.Static) teleportTimerCooldown -= Time.deltaTime;
        if(teleportTimerCooldown <= 0)
        {
            myrigidbody.bodyType = RigidbodyType2D.Dynamic;
            teleportTimerCooldown = teleportTimer;
        }
        //-------------------------------

        Hook();
        Glide();
        ToggleNoClip();
        //---TODO: figure out if this works or not, if i even need this
        if(myrigidbody.velocity.x > 60 || myrigidbody.velocity.x < -60) 
        {
            myrigidbody.velocity = new Vector2(40, myrigidbody.velocity.y);
            Debug.Log("Player had large horizontal velocity spike-------------------------------------------------------------------------");
        }
        //-------------------------------------------------------------

        if(Input.GetKey(KeyCode.P)) GiveAllSkills();
        HorizontalVelocity = myrigidbody.velocity.x;
        if(isGrounded()) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= Time.deltaTime;

        //---TODO: figure out if this works or not, if i even need this
        if(_moveInput.x != 0 && !isWalled() && pHM.canMove) transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * Mathf.Sign(_moveInput.x), transform.localScale.y);
        else if(myrigidbody.velocity.x != 0) Rotate();
        //-------------------------------------------------------------

        wallMovement();
        Crouch();
        checkConditions();
        
        fallSpeed = myrigidbody.velocity.y;
        if(hasDoubleJump) 
        {
            jumpIndex = 0;
            constDJI = 1;
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
        else myrigidbody.gravityScale = 7f;
        Jump();
        LastOnWallTime -= Time.deltaTime;
        if(!isJumping()) LastOnWallTime = coyoteTime;
    }
    private void FixedUpdate()
    {
        distanceJoint.distance = hookCurrentDistance;
        hookCurrentDistance = Mathf.Clamp(hookCurrentDistance + Input.GetAxisRaw("Vertical") * hookMoveSpeed * Time.fixedDeltaTime, 0, hookMaxDistance);
        
        if(pHM.canMove) Move();
        if(_isDashing) myrigidbody.velocity = new Vector2(transform.localScale.normalized.x * (dashSpeed + Mathf.Abs(myrigidbody.velocity.x)/8), 0);
        if(_isSliding) Slide();
    }
    #region Abilities
    private void Move()
    {
        // _moveInput.x = Input.GetAxisRaw("Horizontal");
		// _moveInput.y = Input.GetAxisRaw("Vertical");
        _moveInput = UserInput.Instance.MoveInput;
        if(_isCrouching) moveSpeed = crouchSpeed;
        else moveSpeed = runSpeed;
        targetSpeed = _moveInput.x * moveSpeed;
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
        && !_isSliding && pHM.canMove)
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
            pHM.canMove = false;

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
            //pHM.canMove = true;
        }

        if(UserInput.Instance._jumpAction.WasPressedThisFrame()) coyoteTimeCounter = 0;

        if ((isGrounded() || isWalled() && _isSliding) && !distanceJoint.enabled)
        {
            doubleJumpIndex = constDJI;
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
                pHM.canMove = true;
                waitToUnlockMovement = 0.3f;
                movementLocked = false;
            }
        }

        if(wallJumpWait)
        {
            wallJumpCooldown -= Time.deltaTime;
            if(wallJumpCooldown < 0) 
            {
                pHM.canMove = true;
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
            fallAttackActive = true;
            fallStartingHeight = transform.position.y;
            if(fallSpeedBeforeCTRL == 0) fallSpeedBeforeCTRL = myrigidbody.velocity.y;
            //myrigidbody.velocity = new Vector2(myrigidbody.velocity.x, -15);
            myrigidbody.AddForce(new Vector2(0, -15));
        }
        if(UserInput.Instance._crouchAction.WasReleasedThisFrame() && !isGrounded())
        {
            fallAttackActive = false;
            myrigidbody.velocity = new Vector2(myrigidbody.velocity.x, fallSpeedBeforeCTRL);
        }
        if(!UserInput.Instance._crouchAction.IsPressed() && !isUnderTerrain() || isJumping())
        {
            anim.SetBool("isCrouching", false);
            collCrouch.enabled = false;
            coll.enabled = true;
        }
        if(isGrounded() && fallAttackActive)
        {
            fallSpeedBeforeCTRL = 0;
            fallEndingHeight = transform.position.y;
            fallHeight = fallStartingHeight - fallEndingHeight;
        }
        else fallHeight = 0;
        if(fallHeight >= heightToDealDamage && isGrounded() && isCrouching())
        {
            fastFallAttack();
            gonnaAttack = true;
        }
        else gonnaAttack = false;
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
        charAtt.fastFallAttack = true;
        StartCoroutine(charAtt.hitEnemies());
    }  
    public void Hook()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerToMouseDirection = (mousePos - transform.position).normalized;        
        if(_hookInput)
        {
            distanceJoint.enabled = false;
            hookHit = Physics2D.Raycast(transform.position, playerToMouseDirection, hookMaxDistance, hookableLayers);
            hookHitMovingTarget = Physics2D.Raycast(transform.position, playerToMouseDirection, hookMaxDistance, hookableMovingLayers);
            if(hookHitMovingTarget.collider != null)
            {  
                Debug.Log("Hooked to moving target"); 
                hookCurrentDistance = hookHit.distance;
                distanceJoint.enabled = true;
                distanceJoint.connectedBody = hookHit.rigidbody;
                distanceJoint.connectedAnchor = Vector2.zero;
                doubleJumpIndex = constDJI + 1;
            }
            else if(hookHit.collider != null)
            {   
                Debug.Log("Hooked to static target"); 
                hookCurrentDistance = hookHit.distance;
                distanceJoint.enabled = true;
                distanceJoint.connectedBody = null;
                distanceJoint.connectedAnchor = hookHit.point;
                doubleJumpIndex = constDJI + 1;
            }
        }
        if((_jumpInput || _isDashing || Input.GetKeyDown(KeyCode.LeftControl) || pHM.playerDead) && distanceJoint.enabled) distanceJoint.enabled = false;
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
        if(displayWhereHit.distance > hookMaxDistance) displayHitSprite.color = Color.red;
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
		if(pHM.canMove) myrigidbody.AddForce(movement * Vector2.up);
    }
    public void Glide()
    {
        if(UserInput.Instance._glideAction.IsPressed() && !distanceJoint.enabled) myrigidbody.velocity = new Vector2(myrigidbody.velocity.x, -3.5f);
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
        if(_crouchInput && isGrounded() || isUnderTerrain() && collCrouch.enabled == true) _isCrouching = true;
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
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("player"), LayerMask.NameToLayer("enemyProjectile"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("player"), LayerMask.NameToLayer("enemy"), false);
        yield return new WaitForSeconds(dashCooldownTime);
        canDash = true;
    }    
    IEnumerator Acceleration()
    {
        startAcceleration = false;
        yield return new WaitForSeconds(timeUntilAcceleration);
        startAcceleration = true;
    }
    IEnumerator CheckWallJump()
    {
        yield return new WaitForSeconds(0.3f);
        if(doubleJumpIndex != constDJI && !distanceJoint.enabled) doubleJumpIndex = constDJI;
    }
    #endregion
    #region Gizmos
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(coll.bounds.center - transform.up * maxDistance, boxSize + new Vector3(1f,0,0));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(coll.bounds.center - transform.right * maxDistance * Mathf.Sign(transform.localScale.x) * -1, wallBoxSize);
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

