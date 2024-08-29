using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingTrap : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _damage;
    [SerializeField] private int _currentMovementPoint;
    [SerializeField] private int _numberOfMovementPoints;
    [SerializeField] private GameObject[] _movementPoints;
    [SerializeField] private bool _cycleMovement;
    [SerializeField] private bool _moveBackwards;
    [SerializeField] private Rigidbody2D _rigidbody;
    private Vector2 _movementDirection;

    void Start()
    {
        _numberOfMovementPoints = _movementPoints.Length;
        ChangeVector();
        Move();
    }

    void Update()
    {
        if(Vector2.Distance(transform.position, _movementPoints[_currentMovementPoint].transform.position) <= 0.5f)
        {
            if(_cycleMovement)
            {
                if(_currentMovementPoint == 0) _moveBackwards = false;
                else if(_currentMovementPoint >= _numberOfMovementPoints - 1) _moveBackwards = true;

                if(_moveBackwards) _currentMovementPoint--;
                else _currentMovementPoint++;
                ChangeVector();
            }
            else
            {
                if(_currentMovementPoint >= _numberOfMovementPoints - 1) _currentMovementPoint = 0;
                else _currentMovementPoint++;
                ChangeVector();
            }
            Move();
        }        
    }  

    void ChangeVector()
    {
        _movementDirection = (_movementPoints[_currentMovementPoint].transform.position - transform.position).normalized * _moveSpeed;
    }
    
    public void Move()
    {
        _rigidbody.velocity = _movementDirection;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerHealthManager.Instance.getDamage(_damage);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;            
        for(int i = 0; i < _movementPoints.Length; i++)
        {
            Gizmos.DrawWireSphere(_movementPoints[i].transform.position, 0.5f);
            if(i+1 < _movementPoints.Length) Gizmos.DrawLine(_movementPoints[i].transform.position, _movementPoints[i+1].transform.position);
            else if(!_cycleMovement) Gizmos.DrawLine(_movementPoints[i].transform.position, _movementPoints[0].transform.position);
        }    

    }
}
