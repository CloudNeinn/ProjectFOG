using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planeSwitch : MonoBehaviour
{
    // this is most likely for enemies only
    [SerializeField] private float _switchDelay;
    [SerializeField] private float _switchDelayTimer;

    [SerializeField] private float _checkCollisionRadius;
    [SerializeField] private float _findFreeSpaceRadius;
    private Vector2 _teleportPosition;
    [SerializeField] private LayerMask _collisionLayer;
    public Collider2D[] hitColliders;
    public bool _inOtherWorld {get; private set;}
    [SerializeField] private float _teleportTimer;
    [SerializeField] private float _teleportTimerCooldown;
    [SerializeField] private float _searchStep;
    [SerializeField] private int _maxSearchAttempts;
    private float _searchRadius;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.swithcEnemyPlaneEvent += SwitchPlane;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchPlane()
    {
        //switch plane
        if(_switchDelayTimer <= 0)
        {
            if(_inOtherWorld) _teleportPosition = new Vector2(transform.position.x, transform.position.y - 510);
            else _teleportPosition = new Vector2(transform.position.x, transform.position.y + 510);
            transform.position = CheckCollisionOnTeleport(_teleportPosition);
            _inOtherWorld = !_inOtherWorld;
            _switchDelayTimer = _switchDelay;
        }
        else
        {
            _switchDelayTimer -= Time.deltaTime;
        }
    }

    public Vector3 CheckCollisionOnTeleport(Vector3 TeleportPosition)
    {
        Debug.Log("Check collision on teleport");
        FindFreeSpaceOnTeleport(TeleportPosition);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(TeleportPosition, _checkCollisionRadius, _collisionLayer);
        hitColliders = colliders;
        if(colliders.Length > 0) TeleportPosition = FindFreeSpaceOnTeleport(TeleportPosition);
        return TeleportPosition;
    }

    public Vector3 FindFreeSpaceOnTeleport(Vector3 TeleportPosition)
    {
        Debug.Log("Finding free space for teleport");
        _searchRadius = _findFreeSpaceRadius;

        for (int i = 0; i < _maxSearchAttempts; i++)
        {
            _searchRadius += _searchStep;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(TeleportPosition, _searchRadius, _collisionLayer);
            if (colliders.Length == 0)
            {
                return TeleportPosition;
            }

            TeleportPosition += Random.insideUnitSphere * _searchStep;
        }
        return TeleportPosition;
    }
}
