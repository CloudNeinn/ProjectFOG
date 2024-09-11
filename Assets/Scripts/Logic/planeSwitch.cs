using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planeSwitch : MonoBehaviour
{
    // this is most likely for enemies only
    [SerializeField] private float _switchDelay;
    [SerializeField] private float _switchDelayTimer;

    [SerializeField] private float _teleportTimer;
    [SerializeField] private float _teleportTimerCooldown;
    [SerializeField] private float _searchStep;
    [SerializeField] private int _maxSearchAttempts;
    [SerializeField] private float _checkCollisionRadius;
    [SerializeField] private float _findFreeSpaceRadius;
    [SerializeField] private LayerMask _collisionLayer;
    [SerializeField] private bool _switchPlane;
    private Vector2 _teleportPosition;
    public Collider2D[] hitColliders;
    private float _searchRadius;
    public bool _inOtherWorld {get; private set;}

    void Start()
    {
        _switchPlane = false;
        EventManager.swithcEnemyPlaneEvent += SwitchPlane;
    }

    void Update()
    {
        if(_switchPlane)
        {
            if(_switchDelayTimer <= 0)
            {
                if(_inOtherWorld) _teleportPosition = new Vector2(transform.position.x, transform.position.y - 510);
                else _teleportPosition = new Vector2(transform.position.x, transform.position.y + 510);
                transform.position = CheckCollisionOnTeleport(_teleportPosition);
                _inOtherWorld = !_inOtherWorld;
                _switchDelayTimer = _switchDelay;
                _switchPlane = false;
            }
            else
            {
                _switchDelayTimer -= Time.deltaTime;
            }
        }
    }

    public void SwitchPlane()
    {
        _switchPlane = true;
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
