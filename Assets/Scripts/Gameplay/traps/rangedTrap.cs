using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangedTrap : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private bool _onTimer;
    [SerializeField] private float _timer;
    [SerializeField] private float _cooldown;
    [SerializeField] private int _trapID;
    // Start is called before the first frame update
    void Start()
    {
        //_cooldown = _timer;
        EventManager.shootTrapEvent += Shoot;
    }

    // Update is called once per frame
    void Update()
    {
        if(_cooldown > 0) _cooldown -= Time.deltaTime;
        else if(_onTimer) Shoot(_trapID);
    }

    void Shoot(int TriggerID)
    {
        if(_trapID == TriggerID)
        {
            Instantiate(_projectile, new Vector3(transform.position.x + 0.9f * Mathf.Sign(transform.localScale.x),
            transform.position.y, transform.position.z), Quaternion.identity, this.transform);
            _cooldown = _timer;
        }
    } 
}
