using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create magic missiles")]
public class MagicMissilesItem : Item
{
    [SerializeField] private GameObject magicMissilesPrefab;
    [SerializeField] private bool _shoot;
    [SerializeField] private int _numberOfProjectiles;
    private int _numberOfProjectileShot;
    [SerializeField] private float _timerBetweenProjectiles;
    [SerializeField] private float _cooldownBetweenProjectiles;
    [SerializeField] private float _timerAbilityUsage;
    [SerializeField] private float _cooldownAbilityUsage;
    public override void Execute()
    {
        if(characterControl.Instance._activeItem1Input && _cooldownAbilityUsage <= 0 && !_shoot)
        {
            _numberOfProjectileShot = 0;
            _shoot = true;
        }
        if(_shoot)
        {
            if(_cooldownBetweenProjectiles <= 0) 
            {
                ObjectPoolManager.SpawnObject(magicMissilesPrefab, characterControl.Instance.transform.position + Vector3.up * 1.5f, Quaternion.identity, ObjectPoolManager.PoolType.PlayerProjectileObjects);
                _numberOfProjectileShot++;
                _cooldownBetweenProjectiles = _timerBetweenProjectiles;
            }
            else _cooldownBetweenProjectiles -= Time.deltaTime;
        }
        if(_numberOfProjectileShot >= _numberOfProjectiles)
        {
            _shoot = false;
            _numberOfProjectileShot = 0;
            _cooldownAbilityUsage = _timerAbilityUsage;
        }
        if(_cooldownAbilityUsage > 0) _cooldownAbilityUsage -= Time.deltaTime;
    }
}
