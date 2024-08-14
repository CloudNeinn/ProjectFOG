using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectsInfo> ObjectPools = new List<PooledObjectsInfo>();

    private static GameObject _bloodCoinObjectEmpty;
    private static GameObject _trapProjectileObjectEmpty;
    private static GameObject _enemyProjectileObjectEmpty;

    public enum PoolType
    {
        BloodCoinObjects,
        TrapProjectileObjects,
        EnemyProjectileObjects,
        None
    }

    public static PoolType PoolingType;

    private void Awake()
    {
        SetupEmpties();
    }

    private void SetupEmpties()
    {
        _bloodCoinObjectEmpty = new GameObject("BloodCoinObjects");
        _bloodCoinObjectEmpty.transform.SetParent(this.transform);

        _trapProjectileObjectEmpty = new GameObject("TrapProjectileObjects");
        _trapProjectileObjectEmpty.transform.SetParent(this.transform);

        _enemyProjectileObjectEmpty = new GameObject("EnemyProjectileObjects");
        _enemyProjectileObjectEmpty.transform.SetParent(this.transform);
    }
    
    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
    {
        PooledObjectsInfo pool = ObjectPools.Find(x => x.LookupString == objectToSpawn.name);

        if(pool == null)
        {
            pool = new PooledObjectsInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

        if(spawnableObj == null)
        {
            GameObject parentObject = SetParentObject(poolType);

            spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

            if(parentObject != null)
            {
                spawnableObj.transform.SetParent(parentObject.transform);
            }
        }
        else
        {
            spawnableObj.transform.position = spawnPosition;
            spawnableObj.transform.rotation = spawnRotation;
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        return spawnableObj;
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Transform parentTransform)
    {
        PooledObjectsInfo pool = ObjectPools.Find(x => x.LookupString == objectToSpawn.name);

        if(pool == null)
        {
            pool = new PooledObjectsInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

        if(spawnableObj == null)
        {
            spawnableObj = Instantiate(objectToSpawn, parentTransform);
        }
        else
        {
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        return spawnableObj;
    }

    public static void ReturnObjectToPool(GameObject obj)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7);

        PooledObjectsInfo pool = ObjectPools.Find(x => x.LookupString == goName);     

        if(pool == null)
        {
            Debug.LogWarning("No pool found for object: " + obj.name);
        }   
        else 
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        switch(poolType)
        {
            case PoolType.BloodCoinObjects:
                return _bloodCoinObjectEmpty;
            case PoolType.TrapProjectileObjects:
                return _trapProjectileObjectEmpty;
            case PoolType.EnemyProjectileObjects:
                return _enemyProjectileObjectEmpty;
            case PoolType.None:
                return null;
            default:
                return null;
        }
    }
}

public class PooledObjectsInfo
{
    public string LookupString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}