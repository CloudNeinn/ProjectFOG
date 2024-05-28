using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPatrolPoints : MonoBehaviour
{
    public EnemyBase childScript;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        for(int i = 0; i < childScript.PatrolPoints.Length; i++)
        {
            Gizmos.DrawWireSphere(childScript.PatrolPoints[i].transform.position, 0.5f);
            if(i+1 < childScript.PatrolPoints.Length) Gizmos.DrawLine(childScript.PatrolPoints[i].transform.position, childScript.PatrolPoints[i+1].transform.position);
            else Gizmos.DrawLine(childScript.PatrolPoints[i].transform.position, childScript.PatrolPoints[0].transform.position);
        }     
    }
}
