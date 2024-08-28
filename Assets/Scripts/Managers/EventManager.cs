using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action<int> shootTrapEvent; 
    public static event Action<string> openDoorEvent; 

    void Update()
    {
        
    }

    public static void ShootTrap(int trapID)
    {
        shootTrapEvent?.Invoke(trapID);
    }

    public static void OpenDoor(string doorID)
    {
        openDoorEvent?.Invoke(doorID);
    }
}
