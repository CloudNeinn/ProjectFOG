using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class enemyPatrol : MonoBehaviour
{
    private int htp;

    [Header ("Movement parameters")]
    public float moveSpeed;
    public float noticedSpeed;
    public float patrolSpeed;
    public int dir;
    public int checkdir;
    public float timeToStand;
    public bool hasToPatrol;
    public bool canMove;
    public Vector2 velocity;

    [Header ("Patrol points")]
    public GameObject LeftPoint;
    public GameObject RightPoint;

    [Header ("References")]
    public Rigidbody2D enemyRigidbody;
    private enemyVision eneVis;
    private enemyHealth eneHea;
    public spawnerScript enemiesSpawnPoint;
    // Start is called before the first frame update
    public void Start()
    {
        canMove = true;
        if(LeftPoint == null && RightPoint == null)
        {
            spawnerScript[] spawnPoints = FindObjectsOfType<spawnerScript>();
            float closestDistance = Mathf.Infinity;
            foreach (spawnerScript spawnPoint in spawnPoints)
            {
                float distance = Vector3.Distance(transform.position, spawnPoint.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    LeftPoint = spawnPoint.LeftPoint;
                    RightPoint = spawnPoint.RightPoint;
                    enemiesSpawnPoint = spawnPoint;
                }
            }
        }

        eneHea = GetComponent<enemyHealth>();
        htp = 1;
        if(hasToPatrol)
        {
            eneVis = GetComponent<enemyVision>();
            if(Math.Abs(LeftPoint.transform.position.x - transform.position.x) <= Math.Abs(RightPoint.transform.position.x - transform.position.x)) dir = -1;
            else dir = 1;
        }
        else 
        {
            htp = 0;
        }
    }

    // Update is called once per frame
    void Update()
    { 
        Rotate();
        velocity = enemyRigidbody.velocity;
        if(hasToPatrol)
        {
            if(htp == 0)
            {
                Start();
                htp = 1;
            }
            if(!(eneHea.attacked)) 
            {
                Move(dir); 
                StartCoroutine(standingTime());
            }
        }
    }

    void Move(int direction)
    {
        if(!eneVis.isAttacking && canMove)enemyRigidbody.velocity = new Vector2(moveSpeed * direction, enemyRigidbody.velocity.y);
        else StartCoroutine(standingTime());
        //enemyRigidbody.AddForce(new Vector2(moveSpeed * direction, enemyRigidbody.velocity.y));
    }

    public void Stand()
    {
        enemyRigidbody.velocity = new Vector2(0, enemyRigidbody.velocity.y);
        //enemyRigidbody.AddForce(new Vector2(0, enemyRigidbody.velocity.y));        
    }

    public void Rotate()
    {
        if (enemyRigidbody.velocity.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (enemyRigidbody.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(LeftPoint.transform.position, RightPoint.transform.position);
        Gizmos.DrawWireSphere(LeftPoint.transform.position, 0.5f);
        Gizmos.DrawWireSphere(RightPoint.transform.position, 0.5f);
    }

    IEnumerator standingTime()
    {
        if(transform.position.x <= LeftPoint.transform.position.x)
        {
            Stand();
            yield return new WaitForSeconds(timeToStand);            
            dir = 1;
            Move(dir);
        }
        if(transform.position.x >= RightPoint.transform.position.x)
        {
            Stand();
            yield return new WaitForSeconds(timeToStand);
            dir = -1;
            Move(dir);
        }   
    }
}
