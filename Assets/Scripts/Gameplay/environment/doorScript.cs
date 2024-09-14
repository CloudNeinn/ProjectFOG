using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour, IDataPersistance
{
    [field: SerializeField] public string _doorID {get; private set;}
    public bool isOpen;
    private bool activate;
    [ContextMenu("Generate door id")]
    private void GenerateGuid()
    {
        _doorID = System.Guid.NewGuid().ToString();
    }
    [SerializeField] private int _openDir;
    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("door"), LayerMask.NameToLayer("ground"), true);
        EventManager.openDoorEvent += Open;
        EventManager.closeDoorEvent += Close;
    }

    void Update()
    {
        if(activate) Open(_doorID);
    }
    
    void Open(string DoorID)
    {
        if(_doorID == DoorID && !isOpen)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + transform.localScale.y * 0.75f * _openDir, transform.position.z);
            EventManager.openDoorEvent -= Open;
            EventManager.closeDoorEvent += Close;
            isOpen = true;
            activate = false;
        }
    }

    void Close(string DoorID)
    {
        if(_doorID == DoorID && isOpen)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + transform.localScale.y * -0.75f * _openDir, transform.position.z);
            EventManager.openDoorEvent += Open;
            EventManager.closeDoorEvent -= Close;
            isOpen = false;
            activate = false;
        }
    }

    public void LoadData(GameData data)
    {
        data.doorCondition.TryGetValue(_doorID, out activate);
    }

    public void SaveData(ref GameData data)
    {
        if(data.doorCondition.ContainsKey(_doorID))
        {
            data.doorCondition.Remove(_doorID);
        }
        data.doorCondition.Add(_doorID, isOpen);
    }
}
