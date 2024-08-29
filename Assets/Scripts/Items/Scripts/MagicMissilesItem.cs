using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create magic missiles")]
public class MagicMissilesItem : Item
{
    [SerializeField] private GameObject magicMissilesPrefab;
    public override void Execute()
    {
        if(characterControl.Instance._activeItem1Input)
        {
            ObjectPoolManager.SpawnObject(magicMissilesPrefab, characterControl.Instance.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.PlayerProjectileObjects);
        }
    }
}
