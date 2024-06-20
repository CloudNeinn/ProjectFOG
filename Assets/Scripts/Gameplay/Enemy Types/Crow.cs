using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : EnemyAttacker
{
    [Header ("Crow Options")]
    public float heightAbovePlayer;
    public float radius;
    public float flySway;
    public bool start;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(start) CircleAroundPlayer();
    }

    void CircleAroundPlayer()
    {
        Vector3 playerPos = charCon.transform.position;
        Vector3 newPos = new Vector3(playerPos.x + Mathf.Cos(Time.time * flySway) * radius, playerPos.y + heightAbovePlayer, playerPos.z + Mathf.Sin(Time.time * flySway) * radius);
        transform.position = newPos;
    }
}
