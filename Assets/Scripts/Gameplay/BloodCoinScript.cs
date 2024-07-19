using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCoinScript : MonoBehaviour
{
    [SerializeField] private int coinValue;
    [SerializeField] private float flySpeed;
    [SerializeField] private float flyRadius;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private bool _inRadius;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CircleCollider2D collider;
    [SerializeField] private CircleCollider2D trigger;
    private bool wasInRadius;
    private bool isActive;
    [SerializeField] private float cooldownToActivate;
    public int direction;
    public Vector2 spawnPoint;
    public Vector2 startingPoint;
    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position;
        isActive = false;
        cooldownToActivate = 0.25f;
        direction = Random.Range(0, 2) == 0 ? -1 : 1;
        Physics2D.IgnoreCollision(characterControl.Instance.collCrouch, collider, true);
        Physics2D.IgnoreCollision(characterControl.Instance.coll, collider, true);
        Physics2D.IgnoreLayerCollision(16, 16, true);
        Physics2D.IgnoreLayerCollision(16, 3, true);
        transform.localScale = transform.localScale * (0.5f + coinValue / 2f); 
        startingPoint = new Vector2(Random.Range(-1.5f, 2f), Random.Range(-1f, 2f));
        coinValue = Random.Range(1, 4);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActive) 
        {
            transform.position = Vector2.MoveTowards(transform.position, spawnPoint + startingPoint, 2f * flySpeed * Time.deltaTime);
            if((cooldownToActivate -= Time.deltaTime) <= 0) isActive = true;
        }
        if(_inRadius = InRadius() && isActive) transform.position = Vector2.MoveTowards(transform.position, characterControl.Instance.transform.position, flySpeed * Time.deltaTime);
        else rb.velocity = new Vector2( Mathf.Cos(Time.time) * 0.1f, Mathf.Sin(Time.time) * Random.Range(0.1f, 0.2f) * direction);// rb.velocity * 0.5f;
    }

    void FixedUpdate()
    {

    }
    

    bool InRadius()
    {
        return Physics2D.OverlapCircle(transform.position, flyRadius, playerLayer);      
    }

    // void OnCollisionStay2D(Collision2D collision)
    // {
    //     if (collision.gameObject.tag == "Player")
    //     {
    //         CurrencyManager.Instance.addCurrency(coinValue);
    //         //Destroy(gameObject);
    //     }
    // }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CurrencyManager.Instance.addCurrency(coinValue);
            //gameObject.SetActive(false);
            //Destroy(gameObject);
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}
