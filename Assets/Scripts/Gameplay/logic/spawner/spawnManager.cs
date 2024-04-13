using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
    public spawnerScript[] spawners;
    private enemyHealth eneHea;
    // Start is called before the first frame update
    void Awake()
    {
        spawners = FindObjectsOfType<spawnerScript>();
    }

    void Start()
    {
        foreach(spawnerScript spawner in spawners)
        {
            Instantiate(spawner.enemy, spawner.transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Spawn()
    {
        foreach(spawnerScript spawner in spawners)
        {
            if(spawner.enemy != null)
            {
                eneHea = spawner.enemy.GetComponent<enemyHealth>();
                if(spawner.isAliveSpawner == false)
                {
                    Instantiate(spawner.enemy, spawner.transform.position, Quaternion.identity);
                    eneHea.isAlive = true;
                    spawner.isAliveSpawner = true;
                }
            }
        }
    }
}
