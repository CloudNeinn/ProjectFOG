using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeScript : MonoBehaviour
{

    //private playerHealthManager pHM;
    public Rigidbody2D charRigid;
    public float damage;
    public float knockbackX;
    public float knockbackY;
    public bool isVertical;
    // Start is called before the first frame update
    void Start()
    {
        //pHM = GameObject.FindObjectOfType<playerHealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.rotation.z != 0)
        {
            isVertical = true;
        }
        else isVertical = false;
    }

    public int knockbackDirectionX()
    {
        if(characterControl.Instance.transform.position.x < transform.position.x) return 1;
        else return -1;
    }
    public int knockbackDirectionY()
    {
        if(characterControl.Instance.transform.position.y < transform.position.y) return 1;
        else return -1;
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(isVertical)
            {
                charRigid.velocity = new Vector2(knockbackX * knockbackDirectionX() * -1, charRigid.velocity.y);
                playerHealthManager.Instance.getDamage(damage, 0, 0);
            }
            else
            {
                charRigid.velocity = new Vector2(charRigid.velocity.x, knockbackY * knockbackDirectionY() * -1);
                playerHealthManager.Instance.getDamage(damage, 0, 0);                
            }

        }
    }
}
