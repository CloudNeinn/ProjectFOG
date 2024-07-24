using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingBase : MonoBehaviour, IFlyable, IPatrolable
{
    [field: Header ("Movement Options")]
    [field: SerializeField] public float moveSpeed { get; set; }
    [field: SerializeField] public int directionX { get; set; }
    [field: SerializeField] public float flySpeed { get; set; }
    [field: SerializeField] public Vector2 flyDirection { get; set; }
    [field: SerializeField] public bool isStanding { get; set; }

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

    public void Fly(float speed, Vector2 direction)
    {
        _enemyrb.velocity = new Vector2(speed * direction.x, speed * direction.y);
    }   
    public void Stand() 
    {
        _enemyrb.velocity = new Vector2(0, 0);
    } 
    public void Patrol()
    {
        if(Vector2.Distance(transform.position, PatrolPoints[currentPatrolPoint].transform.position) <= 0.5f){
            moveAfterStanding(flyDirection);
            if(standingCooldown <= 0){
                if(currentPatrolPoint >= numberOfPatrolPoints - 1) currentPatrolPoint = 0;
                else currentPatrolPoint++;
            }
        }
        else
        {
            standingCooldown = standingTime;
            Fly(moveSpeed, flyDirection);          
        }
    }
    public void moveAfterStanding(Vector2 dir)
    {
        if(standingCooldown <= 0) {
            Fly(moveSpeed, dir);
        }
        else 
        {
            //Debug.Log("Standing");
            standingCooldown -= Time.deltaTime;
            Stand();
        }
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
        if(canPatrol) flyDirection = ((PatrolPoints[currentPatrolPoint].transform.position - transform.position).normalized);
    }
    public void ChangeDirection(float direction)
    {
        direction = direction / Mathf.Abs(direction);
        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y);
    }

    void Update()
    {
        Rotate();
    }

    void Start()
    {
        _enemyrb = GetComponent<Rigidbody2D>();
        _enemycol = GetComponent<CircleCollider2D>();
        currentPatrolPoint = 0;
        numberOfPatrolPoints = PatrolPoints.Length;
        flyDirection = ((PatrolPoints[currentPatrolPoint].transform.position - transform.position).normalized);
    }

    void FixedUpdate()
    {
        if(isPatroling) Patrol();
    }
}