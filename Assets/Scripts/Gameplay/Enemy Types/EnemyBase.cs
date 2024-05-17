using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IMoveable, IPatrolable
{
    [field: Header ("Movement Options")]
    [field: SerializeField] public float standingTime { get; set; }
    [field: SerializeField] public float standingCooldown { get; set; }
    [field: SerializeField] public float moveSpeed { get; set; }
    [field: SerializeField] public int directionX { get; set; }

    [field: Header ("Patrol Options")]
    [field: SerializeField] public GameObject LeftPoint { get; set; }
    [field: SerializeField] public GameObject RightPoint { get; set; }

    [field: Header ("Conditions")]
    [field: SerializeField] public bool isMoving;
    [field: SerializeField] public bool isPatroling = true;

    [field: Header ("References")]
    [field: SerializeField] protected Rigidbody2D _enemyrb;
    [field: SerializeField] protected CircleCollider2D _enemycol;

    #region Movement

    public void Move(int direction)
    {
        _enemyrb.velocity = new Vector2(moveSpeed * direction, _enemyrb.velocity.y);
    }

    public void Stand() 
    {
        _enemyrb.velocity = new Vector2(0, _enemyrb.velocity.y);
    } 

    public void Rotate()
    {
        if (_enemyrb.velocity.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (_enemyrb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        } 
        directionX = (int) Mathf.Sign(transform.localScale.x);
    }

    public void ChangeDirection(float direction)
    {
        direction = direction / Mathf.Abs(direction);
        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y);
    }

    #endregion

    #region Patrol

    public void Patrol()
    {
        if(transform.position.x <= LeftPoint.transform.position.x) moveAfterStanding(1);
        else if(transform.position.x >= RightPoint.transform.position.x) moveAfterStanding(-1);
        else
        {
            standingCooldown = standingTime;
            Move(directionX);          
        }
    }
    
    void moveAfterStanding(int dir)
    {
        if(standingCooldown <= 0) Move(dir);
        else 
        {
            standingCooldown -= Time.deltaTime;
            Stand();
        }
    }
    
    #endregion
    
    void Start()
    {
        directionX = (int) transform.localScale.x;
    }

    void Update()
    {
       Rotate();
    }

    void FixedUpdate()
    {
        if(isPatroling) Patrol();
    }
}
