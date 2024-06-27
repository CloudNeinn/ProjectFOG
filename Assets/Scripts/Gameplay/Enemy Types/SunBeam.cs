using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBeam : EnemyFlyingBase
{
    public int numberOfRays;
    public float beamWidth;
    public float rotationAngle;
    public bool isHit;
    public RaycastHit2D[] hits;
    public LayerMask raycastLayer;
    
    private bool playerHit;
    private float startRadians;
    private float additionRadians;
    private Vector2 rayVector;


    // Start is called before the first frame update
    void Start()
    {
        hits = new RaycastHit2D[numberOfRays];
        _enemyrb = GetComponent<Rigidbody2D>();
        _enemycol = GetComponent<CircleCollider2D>();
        currentPatrolPoint = 0;
        numberOfPatrolPoints = PatrolPoints.Length;
        flyDirection = ((PatrolPoints[currentPatrolPoint].transform.position - transform.position).normalized);
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfHits();
        Rotate();
        //if is hit then death or damage
        //or instead is hit death or damage
    }

    void CalculateRadians()
    {
        startRadians = (rotationAngle - beamWidth / 2) * (Mathf.PI / 180);
        additionRadians = (beamWidth / numberOfRays) * (Mathf.PI / 180);
    }

    void CheckIfHits()
    {
        CalculateRadians();
        playerHit = false;
        for (int i = 0; i < numberOfRays; i++)
        {
            rayVector = new Vector2(-(float)Mathf.Cos(startRadians + additionRadians * i), (float)Mathf.Sin(startRadians + additionRadians * i));
            hits[i] = Physics2D.Raycast(transform.position, rayVector, 100.0f, raycastLayer);
            Debug.DrawRay(transform.position, rayVector * hits[i].distance, Color.red, 0f);
            if(hits[i].collider.name == "Charachter") playerHit = true;
        }  
        isHit = playerHit;
    }
}
