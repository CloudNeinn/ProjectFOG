using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingRanged : EnemyFlyingAttacker
{
    [field: SerializeField] public GameObject projectile;
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
        // Very stupid implementation
        // TODO: Make it better
        GameObject shotInstance1 = Instantiate(projectile, new Vector3(transform.position.x + 0.9f * transform.localScale.x,
         transform.position.y + 0.9f * transform.localScale.x, transform.position.z), Quaternion.identity);
        shotInstance1.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1);

        GameObject shotInstance2 = Instantiate(projectile, new Vector3(transform.position.x - 0.9f * transform.localScale.x,
         transform.position.y + 0.9f * transform.localScale.x, transform.position.z), Quaternion.identity);
        shotInstance2.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 1);

        GameObject shotInstance3 = Instantiate(projectile, new Vector3(transform.position.x + 0.9f * transform.localScale.x,
         transform.position.y - 0.9f * transform.localScale.x, transform.position.z), Quaternion.identity);
        shotInstance3.GetComponent<Rigidbody2D>().velocity = new Vector2(1, -1);

        GameObject shotInstance4 = Instantiate(projectile, new Vector3(transform.position.x - 0.9f * transform.localScale.x,
         transform.position.y - 0.9f * transform.localScale.x, transform.position.z), Quaternion.identity);
        shotInstance4.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, -1);
    }

    public bool inRange()
    {
        if(transform.localScale.x > 0 && transform.position.x < charCon.transform.position.x || 
        transform.localScale.x < 0 && transform.position.x > charCon.transform.position.x) return Physics2D.OverlapCircle(transform.position, sightRadius, playerLayer);
        else return false;
    }  

    public bool isBehind()
    {
        return Physics2D.OverlapBox(new Vector2(_enemycol.bounds.center.x + behindDistance * -transform.localScale.x,
         _enemycol.bounds.center.y), behindBoxSize, 0, playerLayer);
    }
    public Vector2 getVector()
    {
        if(!charCon) charCon = GameObject.FindObjectOfType<characterControl>();
        Vector3 playerPosition = charCon.transform.position;
        Vector3 enemyPosition = transform.position;
        return new Vector2(playerPosition.x - enemyPosition.x, playerPosition.y - enemyPosition.y).normalized;
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
            attackCooldown -= Time.deltaTime;
            if(attackCooldown <= 0)
            {
                spawnProjectile();
                attackCooldown = attackTime;
            }

        }
        else isPatroling = true;
        
        if(!inSight() && isAlert)
        {
            if(forgetCooldown < 0) isAlert = false;
            else forgetCooldown -= Time.deltaTime;
        }

    }

    void Update()
    {
        Rotate();
        Attack();
    }
}
