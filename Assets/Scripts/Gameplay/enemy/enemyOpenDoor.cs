using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyOpenDoor : MonoBehaviour
{
    [field:SerializeField] public string _doorID {get; private set;} 
    private enemyHealth _enemyHealth;
    void Start()
    {
        _enemyHealth = GetComponent<enemyHealth>();
    }
    void Update()
    {
        if(!_enemyHealth.isAlive) 
        {
            //foreach(string doorID in _doorID)
            //{
                EventManager.OpenDoor(_doorID);
            //}
        }
    }
}
