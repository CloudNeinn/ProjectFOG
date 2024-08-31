using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHomingProjectile : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private float _homingRadius;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private Collider2D[] _foundObjects;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _timeUntilDestroy;
    [SerializeField] private int _attemptsToFindEnemy;
    [SerializeField] private float _timer;
    private Vector3 _diff;
    private float _curDistance;
    private float _distance;
    private GameObject _closesetObject;

    // Start is called before the first frame update
    void OnEnable()
    {
        _attemptsToFindEnemy = 0;
        _timer = _timeUntilDestroy;
        FindClosestEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if(_closesetObject != null) _rigidbody.velocity = (_closesetObject.transform.position - transform.position).normalized * _speed;
        else if(_attemptsToFindEnemy == 0) 
        {
            FindClosestEnemy();
            _attemptsToFindEnemy++;
        }
        else _timer -= Time.deltaTime;
    }

    void FindClosestEnemy()
    {
        _foundObjects = Physics2D.OverlapCircleAll(transform.position, _homingRadius, _enemyLayer);
        if(_foundObjects.Length == 0) 
        {
            FlyRandomDirection();
            return;
        }
        _distance = Mathf.Infinity;
        foreach(Collider2D go in _foundObjects)
        {
            _diff = go.gameObject.transform.position - transform.position;
            _curDistance = _diff.sqrMagnitude;
            if(_curDistance < _distance)
            {
                _distance = _curDistance;
                _closesetObject = go.gameObject;
                //closest = go;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "enemy") other.gameObject.GetComponent<enemyHealth>().Damage(_damage, false);
        if(other.gameObject.tag == "enemy" || other.gameObject.tag == "ground") Explode();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag != "Player") Explode();
    }

    void FlyRandomDirection()
    {
        _rigidbody.velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(0, 1f)).normalized * _speed;
        if(_timer <= 0) Explode();
    }

    void Explode()
    {
        //some interesting animation or explotion effect
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
    
}
