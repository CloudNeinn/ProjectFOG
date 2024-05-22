using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlying : MonoBehaviour, IFlyable, IPatrolable
{
    [field: Header ("Movement Options")]
    [field: SerializeField] public float flySpeed { get; set; }
    [field: SerializeField] public Vector2 flyDirection { get; set; }

    [field: Header ("Patrol Options")]
    [field: SerializeField] public GameObject LeftPoint { get; set; }
    [field: SerializeField] public GameObject RightPoint { get; set; }
    [field: SerializeField] public GameObject[] PatrolPoints { get; set; }
    [field: SerializeField] public int currentPatrolPoint { get; set; }
    [field: SerializeField] public int numberOfPatrolPoints { get; set; }

    [field: Header ("Conditions")]
    [field: SerializeField] public bool isFlying;
    [field: SerializeField] public bool isPatroling = true;

    [field: Header ("References")]
    [field: SerializeField] protected Rigidbody2D _enemyrb;
    [field: SerializeField] protected CircleCollider2D _enemycol;

    #region Movement

    public void Fly(float speed, Vector2 direction)
    {
        _enemyrb.velocity = new Vector2(speed * direction.x, speed * direction.y);
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
        flyDirection = new Vector2(Mathf.Sign(transform.localScale.x), Mathf.Sign(transform.localScale.y));
    }

    public void ChangeDirection(Vector2 direction)
    {
        direction = direction / Mathf.Abs(direction.x);
        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * direction.x, Mathf.Abs(transform.localScale.y) * direction.y);
    }

    #endregion

    #region Patrol

    public void Patrol()
    {
        if (isPatroling)
        {
            if (transform.position.x < LeftPoint.transform.position.x)
            {
                ChangeDirection(Vector2.right);
            }
            else if (transform.position.x > RightPoint.transform.position.x)
            {
                ChangeDirection(Vector2.left);
            }
        }
    }

    #endregion
}