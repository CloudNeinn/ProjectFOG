using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flameTrap : MonoBehaviour
{
    [SerializeField] private bool _onTimer;
    [SerializeField] private float _timer;
    [SerializeField] private float _cooldown;
    [SerializeField] private int _trapID;
    [SerializeField] private bool _Vertical;
    [SerializeField] private Animator _animator;

    void Start()
    {
        //_cooldown = _timer;
    }

    void Update()
    {
        if(_cooldown > 0) _cooldown -= Time.deltaTime;
        else 
        {
            _animator.SetTrigger("Fire");
            _cooldown = _timer;
        }
    } 

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerHealthManager.Instance.getDamage(20, false);
        }
    }
}

