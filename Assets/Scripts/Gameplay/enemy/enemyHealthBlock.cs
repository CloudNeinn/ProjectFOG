using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealthBlock : enemyHealth
{
    public bool isBlocked;
    public float blockedKnockbackStrengthX;
    public float blockedKnockbackStrengthY;
    void Update()
    {
        //if(isBlocked && !attacked) isBlocked = false;
    }

    public override void Damage(int amount)
    {
        if(currentHealth > 0 && vulnerable)
        {
            vulnerable = false;
            attacked = true;
            if(isBlocked) 
            {
                knockbackForce.Set(blockedKnockbackStrengthX * attackDirection(), blockedKnockbackStrengthY); // used to be new Vector2
            }
            else 
            {
                currentHealth -= amount;
                knockbackForce.Set(knockbackStrengthX * attackDirection(), knockbackStrengthY); // used to be new Vector2
            }
            enemyRigidBody.AddForce(knockbackForce);
            //enemyRigidBody.velocity = new Vector2(knockbackStrengthX * attackDirection(), knockbackStrengthY);
            //enemyRigidBody.AddForce(new Vector2(200 * attackDirection(), 150), ForceMode2D.Impulse);    
            if (currentHealth <= 0)
            {
                isAlive = false;
                if(enePat != null) enePat.enemiesSpawnPoint.isAliveSpawner = false;
                Debug.Log("EnemyDied");
                Destroy(gameObject);
            }
            while(vulTimer <= invulnerabilityTime)
            {
                vulTimer += Time.deltaTime;
            }
            vulnerable = true;
            vulTimer = 0;
            StartCoroutine(attackedCooldown());

        }
    }
    public new IEnumerator attackedCooldown()
    {
        yield return new WaitForSeconds(attackedTime);
        attacked = false;
        isBlocked = false;
    }
}
