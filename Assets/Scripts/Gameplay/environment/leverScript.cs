using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leverScript : MonoBehaviour, IDataPersistance
{
    [SerializeField] private string id;

    [ContextMenu("Generate enemy id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    public GameObject Door;
    public bool doorIsOpen = false;
    public float openSpeed;
    public Rigidbody2D doorRB;
    public int direction;
    public float howHighToOpen;
    public Vector3 startingPosition;
    public bool canClose;
    public bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = Door.transform.position;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("door"), LayerMask.NameToLayer("ground"), true);
        doorRB = Door.GetComponent<Rigidbody2D>();      
    }

    // Update is called once per frame
    void Update()
    {
        if(isOpen && isOpen != doorIsOpen) 
        {
            //doorRB.velocity = new Vector2(0, openSpeed * direction);
            //isOpen = false;
        }
        if(characterControl.Instance.transform.position.x <= transform.position.x + 2 && 
        characterControl.Instance.transform.position.x >= transform.position.x - 2 && 
        characterControl.Instance.transform.position.y <= transform.position.y + 2 && 
        characterControl.Instance.transform.position.y >= transform.position.y - 2)
        {
            if(characterControl.Instance._use1Input) //Input.GetKeyDown(KeyCode.E)
            {   //close
                if(doorIsOpen && canClose) doorRB.velocity = new Vector2(0, openSpeed * direction * -1);
                //open
                if(!doorIsOpen) doorRB.velocity = new Vector2(0, openSpeed * direction);                
            }
        }
        if((startingPosition.y - howHighToOpen) >= Door.transform.position.y && !doorIsOpen ||
        (startingPosition.y + howHighToOpen) <= Door.transform.position.y && !doorIsOpen)
        {
            doorRB.velocity = Vector2.zero;
            doorIsOpen = true;
        }
        if(startingPosition.y <= Door.transform.position.y + 0.1 && doorIsOpen &&
        startingPosition.y >= Door.transform.position.y - 0.1)
        {
            doorRB.velocity = Vector2.zero;
            doorIsOpen = false;
        }

    }

    public void LoadData(GameData data)
    {
        data.doorCondition.TryGetValue(id, out isOpen);
        doorIsOpen = isOpen;
        if(isOpen)
        {
            if(direction > 0) Door.transform.position = 
            new Vector3(Door.transform.position.x, 
            startingPosition.y + howHighToOpen, Door.transform.position.z);            else if(direction < 0) Door.transform.position = 
            new Vector3(Door.transform.position.x, 
            startingPosition.y - howHighToOpen, Door.transform.position.z);
            isOpen = false;
        }
    }

    public void SaveData(ref GameData data)
    {
        isOpen = doorIsOpen;
        if(data.doorCondition.ContainsKey(id))
        {
            data.doorCondition.Remove(id);
        }
        data.doorCondition.Add(id, isOpen);
        isOpen = false;
    }
}
