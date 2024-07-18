using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpointManagement : MonoBehaviour
{
    public Vector3 currentCheckpointPosition;
    public checkpoint[] checkIfSaved;
    public bool checkpMenuOpen;
    // Start is called before the first frame update
    void Start()
    {
        checkIfSaved = FindObjectsOfType<checkpoint>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateCheckpoint()
    {
        foreach(checkpoint obj in checkIfSaved)
        {
            if(obj.isActivated)
            {
                checkpMenuOpen = obj.menuOpen;
                obj.checkpointIsSaved = false;
                obj.isActivated = false;
                Debug.Log("Found an object with true myBoolValue: " + obj.name);
            }
        }        
    }
}
