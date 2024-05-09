using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : EnemyCharge
{
    //public characterControl charCon;
    [field: SerializeField] public GameObject projectile;
    [field: SerializeField] public Animator ani;
    public bool _isBehind;

    void Start()
    {
        directionX = (int) transform.localScale.x;
        charCon = GameObject.FindObjectOfType<characterControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlert) Rotate();
        if(!isAlert) Patrol();
        Attack();
        _isBehind = isBehind();
    }

    public void Attack()
    {
        if(playerInSight()) 
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
                    Move(getDirectionAwayFromPlayer());
                    ChangeDirection(-getDirectionAwayFromPlayer());
                }
            }
        }
        if(!playerInSight() && isAlert)
        {
            if(forgetCooldown < 0) isAlert = false;
            else forgetCooldown -= Time.deltaTime;
        }
        
    }

    public bool playerInSight()
    {        
        if(playerInRange()) 
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

    public bool playerInRange()
    {
        if(transform.localScale.x > 0 && transform.position.x < charCon.transform.position.x || 
        transform.localScale.x < 0 && transform.position.x > charCon.transform.position.x) return Physics2D.OverlapCircle(transform.position, radius, playerLayer);
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
}
