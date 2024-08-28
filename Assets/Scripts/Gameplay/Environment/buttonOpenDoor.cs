using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonOpenDoor : MonoBehaviour
{
    [SerializeField] private string _doorID;
    [SerializeField] private bool _isOpen;
    
    void Update()
    {
        if(characterControl.Instance.transform.position.x <= transform.position.x + 2 && 
        characterControl.Instance.transform.position.x >= transform.position.x - 2 && 
        characterControl.Instance.transform.position.y <= transform.position.y + 2 && 
        characterControl.Instance.transform.position.y >= transform.position.y - 2 &&
        characterControl.Instance._use1Input)
        {
            OpenDoor();
        }
    }
    
    void OpenDoor()
    {
        EventManager.OpenDoor(_doorID);
    }
}
