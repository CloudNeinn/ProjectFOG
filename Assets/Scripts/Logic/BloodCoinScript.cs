using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCoinScript : MonoBehaviour
{
    [SerializeField] public int _coinValue { get; private set; }
    [SerializeField] private float _flySpeed;
    [SerializeField] private float _flyRadius;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private bool _inRadius;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private CircleCollider2D _collider;
    [SerializeField] private CircleCollider2D _trigger;
    private bool _wasInRadius;
    private bool _isActive;
    [SerializeField] private float _cooldownToActivate;
    public int direction;
    public Vector2 spawnPoint;
    public Vector2 startingPoint;
    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position;
        Physics2D.IgnoreCollision(characterControl.Instance.collCrouch, _collider, true);
        Physics2D.IgnoreCollision(characterControl.Instance.coll, _collider, true);
        Physics2D.IgnoreLayerCollision(16, 16, true);
        Physics2D.IgnoreLayerCollision(16, 3, true);
        
    }

    void OnEnable()
    {
        _isActive = false;
        _cooldownToActivate = 0.25f;
        _coinValue = Random.Range(1, 4);
        startingPoint = new Vector2(Random.Range(-1.5f, 2f), Random.Range(-1f, 2f));
        transform.localScale = transform.localScale.normalized * (0.5f + _coinValue / 2f); 
        direction = Random.Range(0, 2) == 0 ? -1 : 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_isActive) 
        {
            transform.position = Vector2.MoveTowards(transform.position, spawnPoint + startingPoint, 2f * _flySpeed * Time.deltaTime);
            if((_cooldownToActivate -= Time.deltaTime) <= 0) _isActive = true;
        }
        if(_inRadius = InRadius() && _isActive) transform.position = Vector2.MoveTowards(transform.position, characterControl.Instance.transform.position, _flySpeed * Time.deltaTime);
        else _rb.velocity = new Vector2( Mathf.Cos(Time.time) * 0.1f, Mathf.Sin(Time.time) * Random.Range(0.1f, 0.2f) * direction);// rb.velocity * 0.5f;
    }

    void FixedUpdate()
    {

    }
    

    bool InRadius()
    {
        return Physics2D.OverlapCircle(transform.position, _flyRadius, _playerLayer);      
    }

    // void OnCollisionStay2D(Collision2D collision)
    // {
    //     if (collision.gameObject.tag == "Player")
    //     {
    //         CurrencyManager.Instance.addCurrency(_coinValue);
    //         //Destroy(gameObject);
    //     }
    // }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CurrencyManager.Instance.addCurrency(_coinValue);
            //gameObject.SetActive(false);
            //Destroy(gameObject);
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}
