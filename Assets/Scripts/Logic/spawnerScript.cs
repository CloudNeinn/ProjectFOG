using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerScript : MonoBehaviour
{
    public GameObject enemy;
    public GameObject LeftPoint;
    public GameObject RightPoint;
    public bool isAliveSpawner;
    private enemyHealth eneHea;
    // Start is called before the first frame update
    void Start()
    {
        isAliveSpawner = true;
        eneHea = enemy.GetComponent<enemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(LeftPoint.transform.position, RightPoint.transform.position);
        Gizmos.DrawWireSphere(LeftPoint.transform.position, 0.5f);
        Gizmos.DrawWireSphere(RightPoint.transform.position, 0.5f);
    }
}
