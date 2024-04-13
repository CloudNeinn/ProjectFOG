using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform endPosition;
    [SerializeField]
    private Transform startPosition;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int direction;
    [SerializeField]
    private float distance;

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, getTarget(), speed * Time.deltaTime);
        distance = (getTarget() - (Vector2)transform.position).magnitude;
        if(distance <= 0.1f) direction *= -1;
    }
    Vector2 getTarget()
    {   
        if(direction == 1) return startPosition.position;
        else return endPosition.position;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(this.transform);
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(null);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(startPosition.position, transform.position);
        Gizmos.DrawLine(endPosition.position, transform.position);
        Gizmos.DrawWireSphere(startPosition.position, 0.5f);
        Gizmos.DrawWireSphere(endPosition.position, 0.5f);
    }
}
