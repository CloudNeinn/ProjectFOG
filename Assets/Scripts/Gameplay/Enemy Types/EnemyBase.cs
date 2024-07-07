using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IWalkable, IPatrolable
{
    [field: Header ("Movement Options")]
    [field: SerializeField] public float moveSpeed { get; set; }
    [field: SerializeField] public int directionX { get; set; }

    [field: Header ("Patrol Options")]
    [field: SerializeField] public GameObject LeftPoint { get; set; }
    [field: SerializeField] public GameObject RightPoint { get; set; }
    [field: SerializeField] public GameObject[] PatrolPoints { get; set; }
    [field: SerializeField] public int currentPatrolPoint { get; set; }
    [field: SerializeField] public int numberOfPatrolPoints { get; set; }
    [field: SerializeField] public float standingTime { get; set; }
    [field: SerializeField] public float standingCooldown { get; set; }
    [field: SerializeField] public bool canPatrol { get; set; }

    [field: Header ("Conditions")]
    [field: SerializeField] public bool isMoving;
    [field: SerializeField] public bool isPatroling = true;
    
    [field: Header ("References")]
    [field: SerializeField] protected Rigidbody2D _enemyrb;
    [field: SerializeField] protected CircleCollider2D _enemycol;

    #region Movement

    public void Walk(int direction)
    {
        _enemyrb.velocity = new Vector2(moveSpeed * direction, _enemyrb.velocity.y);
    }

    public void Stand() 
    {
        _enemyrb.velocity = new Vector2(0, _enemyrb.velocity.y);
    } 

    public void Rotate()
    {
        /*if (_enemyrb.velocity.x == 0f)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else */
        if (_enemyrb.velocity.x >= 0.25f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (_enemyrb.velocity.x < -0.25f)
        {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
         
        directionX = (int)Mathf.Sign(((PatrolPoints[currentPatrolPoint].transform.position - transform.position).normalized.x));
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
        if(Vector2.Distance(transform.position, PatrolPoints[currentPatrolPoint].transform.position) <= 0.5f){
            moveAfterStanding(((PatrolPoints[currentPatrolPoint].transform.position - transform.position).normalized));
            if(standingCooldown <= 0){
                if(currentPatrolPoint == numberOfPatrolPoints - 1) currentPatrolPoint = 0;
                else currentPatrolPoint++;
            }
        }
        else
        {
            standingCooldown = standingTime;
            Walk(directionX);          
        }
    }
    
    public void moveAfterStanding(Vector2 dir)
    {
        if(standingCooldown <= 0) {
            Walk((int)Mathf.Sign(dir.x));
        }
        else 
        {
            Debug.Log("Standing");
            standingCooldown -= Time.deltaTime;
            Stand();
        }
    }
    
    #endregion
    void Awake()
    {
        numberOfPatrolPoints = PatrolPoints.Length;
        currentPatrolPoint = 0;
        standingCooldown = standingTime;
        directionX = (int)Mathf.Sign(((PatrolPoints[currentPatrolPoint].transform.position - transform.position).normalized.x));
        //directionX = (int) transform.localScale.x;
    }

    void Update()
    {
       Rotate();
    }

    void FixedUpdate()
    {
        if(isPatroling && canPatrol) Patrol();
    }
/*
    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {

        }
    }*/
}
