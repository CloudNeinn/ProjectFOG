using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleProjectileScript : projectileScript
{
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("enemy"), LayerMask.NameToLayer("enemy"), true);
        canDamagePlayer = true;
        //pHM = GameObject.FindObjectOfType<playerHealthManager>();
        rigid.velocity = getVector() * speed;
        movementVector = getVector();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public new Vector2 getVector()
    {
        if(transform.parent.position.x < transform.position.x) projectileVector = Vector2.right;
        else if(transform.parent.position.x > transform.position.x) projectileVector = Vector2.right * -1;
        else if(transform.parent.position.y < transform.position.y) projectileVector = Vector2.up;
        else projectileVector = Vector2.up * -1;/* if(transform.parent.position.y > transform.position.y)*/
        return projectileVector;

    }
}
