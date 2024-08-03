using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAttack : MonoBehaviour
{
    [Header ("Attack parameters")]
    public float damage;
    public float knockStrengthX;
    public float knockStrengthY;
    public float blockKnockStrengthX;
    public float blockKnockStrengthY;

    [Header ("Condition check")]
    public bool canDamagePlayer;
    //public playerHealthManager pHM;
    private enemyHealth eneHea;

    // Start is called before the first frame update
    void Start()
    {
        canDamagePlayer = true;
        eneHea = GetComponentInParent<enemyHealth>();//transform.parent.
        //pHM = GameObject.FindObjectOfType<playerHealthManager>();  
    }

    IEnumerator OnTriggerEnter2D(Collider2D collision) //maybe OnTriggerStay2D
    {
        if (collision.gameObject.tag == "BlockShield") canDamagePlayer = false;
        if (collision.gameObject.tag == "Player")
        {   
            if(playerHealthManager.Instance != null && eneHea != null) 
            {
                playerHealthManager.Instance.enemy = eneHea;
                yield return new WaitForSeconds(0.1f);
                //playerHealthManager.Instance.canMove = false;
                if(canDamagePlayer) playerHealthManager.Instance.getDamage(damage, knockStrengthX, knockStrengthY);
                else  playerHealthManager.Instance.getDamage(0, blockKnockStrengthX, blockKnockStrengthY);

            }
            
        }
            
    }
    void OnTriggerExit2D(Collider2D collision) 
    {
        if (collision.gameObject.tag == "BlockShield") canDamagePlayer = true;
    }
}
