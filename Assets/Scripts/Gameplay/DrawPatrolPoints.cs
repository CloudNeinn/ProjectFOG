using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPatrolPoints : MonoBehaviour
{
    public EnemyBase baseChildScript;
    public EnemyFlyingBase flyingBaseChildScript;
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if(baseChildScript == null && flyingBaseChildScript == null)
        {
            baseChildScript = GetComponentInChildren<EnemyBase>(true);
            flyingBaseChildScript = GetComponentInChildren<EnemyFlyingBase>(true);
        }
        if(baseChildScript != null) {
            for(int i = 0; i < baseChildScript.PatrolPoints.Length; i++)
            {
                Gizmos.DrawWireSphere(baseChildScript.PatrolPoints[i].transform.position, 0.5f);
                if(i+1 < baseChildScript.PatrolPoints.Length) Gizmos.DrawLine(baseChildScript.PatrolPoints[i].transform.position, baseChildScript.PatrolPoints[i+1].transform.position);
                else Gizmos.DrawLine(baseChildScript.PatrolPoints[i].transform.position, baseChildScript.PatrolPoints[0].transform.position);
            }    
        }
        else if(flyingBaseChildScript != null) {
            for(int i = 0; i < flyingBaseChildScript.PatrolPoints.Length; i++)
            {
                Gizmos.DrawWireSphere(flyingBaseChildScript.PatrolPoints[i].transform.position, 0.5f);
                if(i+1 < flyingBaseChildScript.PatrolPoints.Length) Gizmos.DrawLine(flyingBaseChildScript.PatrolPoints[i].transform.position, flyingBaseChildScript.PatrolPoints[i+1].transform.position);
                else Gizmos.DrawLine(flyingBaseChildScript.PatrolPoints[i].transform.position, flyingBaseChildScript.PatrolPoints[0].transform.position);
            }    
        }
    }
}
