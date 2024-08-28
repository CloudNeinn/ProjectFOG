using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bloodPool : MonoBehaviour
{
    [SerializeField] private string _id;
    [ContextMenu("Generate blood pool id")]
    private void GenerateGuid()
    {
        _id = System.Guid.NewGuid().ToString();
    }
    [SerializeField] private bool _isEmpty;
    [SerializeField] private bool _isActive;
    [SerializeField] private float _bloodAmount;
    [SerializeField] private GameObject _blood;
    private GameObject _bloodPoint;
    private float _spawnTimer = 0.1f;
    private float _spawnCooldown;

    void Start()
    {
        if(!_isEmpty) _spawnCooldown = _spawnTimer;
    }

    void Update() 
    {
        if(characterControl.Instance.transform.position.x <= transform.position.x + 4 
        && characterControl.Instance.transform.position.x >= transform.position.x - 4
        && characterControl.Instance.transform.position.y <= transform.position.y + 4 
        && characterControl.Instance.transform.position.y >= transform.position.y - 4 
        && characterControl.Instance._use1Input) _isActive = true;
        
        if(!_isEmpty && _isActive)
        {
            if(_spawnCooldown <= 0) SpawnBloodPoints();
            else _spawnCooldown -= Time.deltaTime;
        }
    }

    private void SpawnBloodPoints()
    {
        if(_bloodAmount > 0)
        {
            _bloodPoint = ObjectPoolManager.SpawnObject(_blood, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.BloodCoinObjects);
            _bloodAmount -= _bloodPoint.GetComponent<BloodCoinScript>()._coinValue;
            _spawnCooldown = _spawnTimer;
        }
        else _isEmpty = true;
    }
    
}
