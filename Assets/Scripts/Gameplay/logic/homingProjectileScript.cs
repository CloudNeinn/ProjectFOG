using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homingProjectileScript : projectileScript
{
    public float timeUntilHoming;
    public float homingDelayTime;
    public float homingDelayCooldown;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("enemy"), LayerMask.NameToLayer("enemy"), true);
        if(!canDamageEnemy) Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("enemyProjectile"), LayerMask.NameToLayer("enemy"), true);
        canDamagePlayer = true;
        pHM = GameObject.FindObjectOfType<playerHealthManager>();
        charCon = GameObject.FindObjectOfType<characterControl>();
        //rigid.velocity = getVector() * speed;
        //movementVector = getVector();
    }
    void Update()
    {
        if(timeUntilHoming <= 0)
        {
            if(homingDelayCooldown <= 0)
            {
                rigid.velocity = getVector() * speed;
                movementVector = getVector();
                homingDelayCooldown = homingDelayTime;
            }
            else homingDelayCooldown -= Time.deltaTime;
        }
        else timeUntilHoming -= Time.deltaTime;
    }
}
