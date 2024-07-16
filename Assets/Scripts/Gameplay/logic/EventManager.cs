using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action<int> shootTrapEvent; 

    void Update()
    {
        
    }

    public static void ShootTrap(int trapID)
    {
        shootTrapEvent?.Invoke(trapID);
    }
}
