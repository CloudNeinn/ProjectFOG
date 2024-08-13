using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript : MonoBehaviour
{
    public float damage;
    public float speed;
    //public playerHealthManager pHM;
    public bool canDamagePlayer;
    public bool canDamageEnemy;
    public Rigidbody2D rigid;
    public Vector2 movementVector;
    public enemyRanged eneRan;
    public enemyHealth eneHea;
    public bool canBeBlocked;
    public bool canBeRedirected;
    public Vector2 projectileVector;
    private Vector3 playerPosition;
    private Vector3 enemyPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("enemy"), LayerMask.NameToLayer("enemy"), true);
        canDamagePlayer = true;
        //pHM = GameObject.FindObjectOfType<playerHealthManager>();
        rigid.velocity = getVector() * speed;
        movementVector = getVector();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && playerHealthManager.Instance != null && canDamagePlayer) playerHealthManager.Instance.getDamage(damage);
        else if(collision.gameObject.tag == "Player" && playerHealthManager.Instance != null && !canDamagePlayer) playerHealthManager.Instance.getDamage(0);
        if (collision.gameObject.tag == "enemy") collision.gameObject.GetComponent<enemyHealth>().Damage(20);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BlockShield" && canBeBlocked) canDamagePlayer = false;
        if (collision.gameObject.tag == "attackTrigger" && canBeRedirected && characterControl.Instance.hasDeflectProjectile) 
        {
            rigid.velocity = new Vector2(0, 0);
            rigid.velocity = new Vector2(movementVector.x * -1, movementVector.y * -1) * speed;
        }
    }

    public Vector2 getVector()
    {
        Vector3 playerPosition = characterControl.Instance.transform.position;
        Vector3 enemyPosition = transform.position;
        return new Vector2(playerPosition.x - enemyPosition.x, playerPosition.y - enemyPosition.y).normalized;
    }
}
