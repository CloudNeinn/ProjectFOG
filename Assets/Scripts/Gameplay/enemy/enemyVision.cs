using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyVision : MonoBehaviour
{
    [Header ("Cooldown Parameters")]
    public float cooldownTimer = Mathf.Infinity;
    public float attackCooldown;
    public float timeUntilForget;
    public float attackTime;
    public float waitTimer;

    [Header ("BoxCast Parameters")]
    public Vector3 attackBoxSize;
    public float attackDistance;
    public Vector3 sightBoxSize;
    public float sightDistance;
    public Vector3 behindBoxSize;
    public float behindDistance;
    public float radius;

    [Header ("Condition check")]
    public bool inRange;
    public bool inSight;
    public bool isBehind;
    public bool hasEnteredBefore;
    public bool attacked;
    public bool isAttacking;
    private bool canAttack;

    [Header ("References")]
    public BoxCollider2D boxCollider;
    public LayerMask playerLayer;
    public LayerMask raycastLayer;
    private enemyPatrol enePat;
    private enemyRanged eneRan;
    private enemyAttack eneAtt;
    public int enemyTypeIndex;
    private enemyAnimations eneAni;
    private enemyBehaviour eneBeh;
    private enemyHealth eneHea;

    // Start is called before the first frame update
    void Start()
    {
        eneHea = GetComponent<enemyHealth>();
        eneBeh = GetComponent<enemyBehaviour>();
        eneAni = GetComponent<enemyAnimations>();
        enePat = GetComponent<enemyPatrol>();
        eneRan = GetComponent<enemyRanged>();
        eneAtt = GetComponent<enemyAttack>();
        enePat.moveSpeed = enePat.patrolSpeed;
        hasEnteredBefore = false;
        if(eneRan == null && eneAtt != null) enemyTypeIndex = 0;
        else if(eneRan != null && eneAtt == null) enemyTypeIndex = 1;
    }

    void FixedUpdate()
    {
        if(playerInRange())
        {   
            StartCoroutine(StandWhileAttacking(attackTime));
            eneAni.startAttackAnimation(true);
        }
        else
        {
            eneAni.startAttackAnimation(false);
            cooldownTimer = 0;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInSight() && !characterControl.Instance.isUnderTerrain())
        {
            enePat.moveSpeed = enePat.noticedSpeed;
            if(eneBeh != null) eneBeh.enabled = true;
            hasEnteredBefore = true;
            inSight = true;
            waitTimer = timeUntilForget;
        }

        if(!playerInSight() && hasEnteredBefore)
        {
            //StartCoroutine(TimeToWait(timeUntilForget));
            waitTimer -= Time.deltaTime;
            if(waitTimer<=0) 
            {
                inSight = false;
                hasEnteredBefore = false;
            }
        }

        if(playerIsBehind())
        {
            if(characterControl.Instance.moveSpeed >= characterControl.Instance.crouchSpeed + 1  || eneHea.attacked)
            {
                enePat.Rotate();
                enePat.dir *= -1;
                enePat.checkdir = enePat.dir;
                eneHea.attacked = false;
            }
        }

        inRange = playerInRange();
        isBehind = playerIsBehind();
    }

    public bool playerInSight()
    {
        if(enemyTypeIndex == 0) return Physics2D.OverlapBox(new Vector2(transform.position.x + sightDistance * -transform.localScale.x, transform.position.y), sightBoxSize, 0, playerLayer);
        else if(enemyTypeIndex == 1)
        {
            if(playerInRange()) 
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, eneRan.getVector(), 100.0f, raycastLayer);
                Debug.DrawRay(transform.position, eneRan.getVector() * hit.distance, Color.red, 0f);
                if(hit.collider.name == "Charachter") 
                {
                    //eneRan.playerPosInSight = hit.point;
                    return true;
                }
                else return false;
            }
            else return false;
        }
        else return false;
    }
    public bool playerInRange()
    {
        if(enemyTypeIndex == 0) return Physics2D.OverlapBox(new Vector2(boxCollider.bounds.center.x + attackDistance * -transform.localScale.x, boxCollider.bounds.center.y), attackBoxSize, 0, playerLayer);
        else if(enemyTypeIndex == 1)
        {
            if(transform.localScale.x == 1 && transform.position.x < characterControl.Instance.transform.position.x || 
            transform.localScale.x == -1 && transform.position.x > characterControl.Instance.transform.position.x) return Physics2D.OverlapCircle(transform.position, radius, playerLayer);
            else return false;
        }
        else return false;
        
    }
    public bool playerIsBehind()
    {
        return Physics2D.OverlapBox(new Vector2(boxCollider.bounds.center.x + behindDistance * -transform.localScale.x, boxCollider.bounds.center.y), behindBoxSize, 0, playerLayer);
    }  

    void OnDrawGizmos()
    {
        if(enemyTypeIndex == 0)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(boxCollider.bounds.center - transform.right * transform.localScale.x * attackDistance, attackBoxSize);      

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(boxCollider.bounds.center - transform.right * transform.localScale.x * sightDistance, sightBoxSize);
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(boxCollider.bounds.center - transform.right * transform.localScale.x * behindDistance, behindBoxSize);
        }
        else if(enemyTypeIndex == 1)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, radius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(boxCollider.bounds.center - transform.right * transform.localScale.x * eneRan.checkGroundDistance, eneRan.checkGroundBoxSize);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(boxCollider.bounds.center - transform.right * transform.localScale.x * eneRan.checkWallDistance, eneRan.checkWallBoxSize);    
        }        
    }

    public IEnumerator TimeToWait(float timeUntilForget)
    {
        yield return new WaitForSeconds(timeUntilForget);
        enePat.moveSpeed = enePat.patrolSpeed;
        if(!playerInSight())inSight = false;
    }

    public IEnumerator StandWhileAttacking(float attackTime)
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackTime);
        isAttacking = false;
    }
}
