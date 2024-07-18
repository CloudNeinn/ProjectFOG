using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyRanged : MonoBehaviour
{
    private enemyBehaviour eneBeh;
    private enemyAnimations eneAni;
    private enemyPatrol enePat;
    public enemyVision eneVis;
    private Vector3 playerPosition;
    private Vector3 enemyPosition;
    public Vector2 projectileVector;
    public float shootCooldown;
    public float shootTimer;
    public bool canShoot;
    public GameObject projectile;
    public bool isShooting;
    public Vector3 playerPosInSight;
    public LayerMask checkGroundLayer;
    public LayerMask checkWallLayer;
    public Vector3 checkGroundBoxSize;
    public float checkGroundDistance;
    public Vector3 checkWallBoxSize;
    public float checkWallDistance;
    public bool checkIfCanMoveVar;
    //public float invCooldown;
    //public float invTimer;

    [Header ("Cooldown Parameters")]
    public float cooldownTimer = Mathf.Infinity;
    public float attackCooldown;
    public float timeUntilForget;
    public float attackTime;
    

    // Start is called before the first frame update
    void Start()
    {
        canShoot = false;
        shootTimer = shootCooldown;
        eneVis = GetComponent<enemyVision>();
        eneBeh = GetComponent<enemyBehaviour>();
        enePat = GetComponent<enemyPatrol>();
        eneAni = GetComponent<enemyAnimations>();
        enePat.moveSpeed = enePat.patrolSpeed;
    }

    void FixedUpdate()
    {
        if(eneVis.playerInSight())
        {
            StartCoroutine(eneVis.StandWhileAttacking(attackTime));
            eneAni.startAttackAnimation(true);
            //enePat.moveSpeed = enePat.noticedSpeed;
            if(eneBeh != null) eneBeh.enabled = true;
            eneVis.hasEnteredBefore = true;
            eneVis.inSight = true;
        }
        else 
        {
            eneVis.inSight = false;
            eneAni.startAttackAnimation(false);
            eneVis.cooldownTimer = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkIfCanMoveVar = checkIfCanMove();
        if(eneVis.playerInSight()) 
        {
            Shooting();
            enePat.canMove = false;
            //enePat.enabled = false;
        }
        else enePat.canMove = true;
        if(playerPosInSight.x < transform.position.x && checkIfCanMove() && !canShoot) enePat.enemyRigidbody.velocity = new Vector2(enePat.moveSpeed * 1, enePat.enemyRigidbody.velocity.y); // move right
        else if(playerPosInSight.x >= transform.position.x && checkIfCanMove() && !canShoot)  enePat.enemyRigidbody.velocity = new Vector2(enePat.moveSpeed * -1, enePat.enemyRigidbody.velocity.y);// move left
        
    }

    public Vector2 getVector()
    {
        float indX;
        float indY;
        float maxInd;
        playerPosition = characterControl.Instance.transform.position;
        enemyPosition = transform.position;
        indX = playerPosition.x - enemyPosition.x;
        indY = playerPosition.y - enemyPosition.y;
        if(Mathf.Abs(indX) >= Mathf.Abs(indY)) maxInd = Mathf.Abs(indX);
        else maxInd = Mathf.Abs(indY);
        indX = indX / maxInd;
        indY = indY / maxInd;
        projectileVector = new Vector2(indX, indY);
        return projectileVector;
    }

    public void Shooting()
    {
        if(!canShoot)
        {
            //if(invTimer <= 0) Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("enemy"), LayerMask.NameToLayer("enemyProjectile"), false);
            //invTimer -= Time.deltaTime;
            isShooting = false;
            shootTimer -= Time.deltaTime;
            if(shootTimer < 0) 
            {
                shootTimer = shootCooldown;
                canShoot = true;
            }
        }
        else if(canShoot)
        {
            enePat.Stand();
            enePat.Rotate();
            canShoot = false;
            isShooting = true;
            //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("enemy"), LayerMask.NameToLayer("enemyProjectile"), true);
            Instantiate(projectile, new Vector3(transform.position.x + 0.75f * transform.localScale.x, transform.position.y, transform.position.z), Quaternion.identity);
            //invTimer = invCooldown;
            Debug.Log("Projectile spawned");
        }
    }

    public bool checkIfGround()
    {
        if(Physics2D.OverlapBox(new Vector2(eneVis.boxCollider.bounds.center.x + checkGroundDistance * -transform.localScale.x, eneVis.boxCollider.bounds.center.y), checkGroundBoxSize, 0, checkGroundLayer)) return true;
        else return false;
    }

    public bool checkIfWall()
    {
        if(Physics2D.OverlapBox(new Vector2(eneVis.boxCollider.bounds.center.x + checkWallDistance * -transform.localScale.x, eneVis.boxCollider.bounds.center.y), checkWallBoxSize, 0, checkWallLayer)) return true;
        else return false;
    }

    public bool checkIfCanMove()
    {
        if(!checkIfWall() && checkIfGround()) return true;
        else return false;
    }
}
