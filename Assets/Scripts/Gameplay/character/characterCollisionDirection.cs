using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterCollisionDirection : MonoBehaviour
{ 
    public static characterCollisionDirection Instance;
    [SerializeField] private Vector2 _horizontalBoxSize;
    [SerializeField] private Vector2 _verticalBoxSize;
    [SerializeField] private Vector3 _centerOffset;
    [SerializeField] private float _boxDistance;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Vector2 _knockbackVector;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _knockbackVector = new Vector2(0, 0);
    }

    void Update()
    {
        //GetKnockbackVector();
    }

    public Vector2 GetKnockbackVector()
    {
        _knockbackVector = Vector2.zero;
        if(Physics2D.OverlapBox((transform.position + _centerOffset) + Vector3.up * _boxDistance, _horizontalBoxSize, 0, _layerMask))
        {
            _knockbackVector.Set(_knockbackVector.x, -1);
        }
        else if(Physics2D.OverlapBox((transform.position + _centerOffset) + Vector3.up * _boxDistance * -1, _horizontalBoxSize, 0, _layerMask))
        {
            _knockbackVector.Set(_knockbackVector.x, 1);
        }
        if(Physics2D.OverlapBox((transform.position + _centerOffset) + Vector3.right * _boxDistance, _verticalBoxSize, 0, _layerMask))
        {
            _knockbackVector.Set(-1, _knockbackVector.y);
        }
        else if(Physics2D.OverlapBox((transform.position + _centerOffset) + Vector3.right * _boxDistance * -1, _verticalBoxSize, 0, _layerMask))
        {
            _knockbackVector.Set(1, _knockbackVector.y);
        }
        return _knockbackVector;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube((transform.position + _centerOffset) + Vector3.up * _boxDistance, _horizontalBoxSize);
        Gizmos.DrawWireCube((transform.position + _centerOffset) + Vector3.right * _boxDistance, _verticalBoxSize);
        Gizmos.DrawWireCube((transform.position + _centerOffset) + (Vector3.right * _boxDistance) * -1, _verticalBoxSize);
        Gizmos.DrawWireCube((transform.position + _centerOffset) + (Vector3.up * _boxDistance) * -1, _horizontalBoxSize);
    }
}
