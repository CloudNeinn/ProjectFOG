using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHomingProjectile : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _homingRadius;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private GameObject[] _foundObjects;
    private Vector3 diff;
    private float curDistance;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        _foundObjects = Physics2D.OverlapCircleAll(transform.position, _homingRadius, _enemyLayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FindClosestEnemy()
    {
        distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach(GameObject go in _foundObjects)
        {
            diff = go.transform.position - position;
            curDistance = diff.sqrMagnitude;
            if(curDistance < distance)
            {
                distance = curDistance;
                //closest = go;
            }
        }
    }
    
}
