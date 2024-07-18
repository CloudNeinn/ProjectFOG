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
    [SerializeField] private bool _Vertical;
    private Vector2 _shootDirection;
    // Start is called before the first frame update
    void Start()
    {
        //_cooldown = _timer;
        if(_Vertical) 
        {
            _shootDirection = Vector2.up * 0.9f;
            if(Mathf.Sign(transform.localScale.y) == 1) transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -90);
            else transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90);
        }
        else _shootDirection = Vector2.right * 0.9f;
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
            Instantiate(_projectile, new Vector3(transform.position.x + _shootDirection.x * Mathf.Sign(transform.localScale.x),
            transform.position.y + _shootDirection.y * Mathf.Sign(transform.localScale.y), transform.position.z), Quaternion.identity, this.transform);
            _cooldown = _timer;
        }
    } 
}
