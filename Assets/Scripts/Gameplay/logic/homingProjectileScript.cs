using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homingProjectileScript : projectileScript
{
    public float timeUntilHoming;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("enemy"), LayerMask.NameToLayer("enemy"), true);
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
            rigid.velocity = getVector() * speed;
            movementVector = getVector();
        }
        else timeUntilHoming -= Time.deltaTime;
    }
}
