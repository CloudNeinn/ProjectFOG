using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Crow : EnemyFlyingChaser
{
    [Header ("Crow Options")]
    public float heightAbovePlayer;
    public float radius;
    public float flySway;
    public bool start;
    public Vector2 lastPlayerPos;

    // Start is called before the first frame update
    void Start()
    {
        enemyStartingPosition = transform.position;
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        if(inSight()) {
            lastPlayerPos = charCon.transform.position;
            isAlert = true;
        }
        if(isAlert) {
            CircleAroundPlayer();
            PathFollow();
        }
    }

    void CircleAroundPlayer()
    {
        if(transform.position.x < lastPlayerPos.x) target = new Vector2(lastPlayerPos.x + flySway, lastPlayerPos.y + heightAbovePlayer);
        else target = new Vector2(lastPlayerPos.x - flySway, lastPlayerPos.y + heightAbovePlayer);
    }
}
