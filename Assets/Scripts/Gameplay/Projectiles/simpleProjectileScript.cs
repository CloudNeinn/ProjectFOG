using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleProjectileScript : projectileScript
{
    public GameObject shooterObject;
    public Vector2 startingPosition;
        
    void Start()
    {
        transform.localScale *= 1.5f;
    }

    void OnEnable()
    {
        //startingPosition = transform.position;
        //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("enemy"), LayerMask.NameToLayer("enemy"), true);
        //canDamagePlayer = true;
        ////pHM = GameObject.FindObjectOfType<playerHealthManager>();
        //movementVector = getVector();
        //rigid.velocity = getVector() * speed;
    }

    public new Vector2 getVector()
    {
        //if(/*shooterObject.*/transform.parent.position.x < transform.position.x) projectileVector = Vector2.right;
        //else if(/*shooterObject.*/transform.parent.position.x > transform.position.x) projectileVector = Vector2.right * -1;
        //else if(/*shooterObject.*/transform.parent.position.y < transform.position.y) projectileVector = Vector2.up;
        //else projectileVector = Vector2.up * -1;/* if(shooterObject.transform.position.y > transform.position.y)*/
        //return projectileVector;
        return Vector2.zero;

    }
}
