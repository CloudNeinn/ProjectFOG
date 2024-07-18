using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    public bool checkpointIsSaved;
    public GameObject popUpSave;
    public GameObject popUpMenu;
    public bool isActivated;
    public playerHealthManager pHM;
    public checkpointManagement checkpManage;
    public DataPersistanceManager dataPerMan;
    private gameplayUI gameUI;
    public Vector3 checkPosition;
    public bool isHealed;
    public bool menuOpen;
    public GameObject checkpMenu;

    // Start is called before the first frame update
    void Start()
    {
        popUpSave = transform.GetChild(0).gameObject;
        popUpMenu = transform.GetChild(1).gameObject;
        pHM = GameObject.FindObjectOfType<playerHealthManager>();
        dataPerMan = GameObject.FindObjectOfType<DataPersistanceManager>();
        checkpManage = GameObject.FindObjectOfType<checkpointManagement>();
        checkpMenu = GameObject.Find("CheckpointMenu");
        gameUI = GameObject.FindObjectOfType<gameplayUI>(); 
        if(characterControl.Instance.transform.position.x <= transform.position.x + 3 
        && characterControl.Instance.transform.position.x >= transform.position.x - 3
        && characterControl.Instance.transform.position.y <= transform.position.y + 3 
        && characterControl.Instance.transform.position.y >= transform.position.y - 3)
        {
            checkpManage.updateCheckpoint();
            checkpManage.currentCheckpointPosition = transform.position;
            checkpointIsSaved = true;
            dataPerMan.SaveGame();
        }
        popUpMenu.SetActive(false);
        //checkpMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        checkPosition = characterControl.Instance.transform.position;
        if(characterControl.Instance.transform.position.x <= transform.position.x + 4 
        && characterControl.Instance.transform.position.x >= transform.position.x - 4
        && characterControl.Instance.transform.position.y <= transform.position.y + 4 
        && characterControl.Instance.transform.position.y >= transform.position.y - 4)
        {
            if(characterControl.Instance._use2Input && !menuOpen 
            && isActivated && Time.timeScale != 0) // Input.GetKeyDown(KeyCode.F)
            {
                menuOpen = true;
                checkpMenu.SetActive(menuOpen);
            }
            else if(characterControl.Instance._use2Input && menuOpen) // Input.GetKeyDown(KeyCode.F)
            {
                menuOpen = false;
                checkpMenu.SetActive(menuOpen);
            }
            //show e popup icon
            if(checkpointIsSaved && characterControl.Instance._use1Input && !isHealed) // Input.GetKeyDown(KeyCode.E)
            {
                gameUI.Respawn();
                isHealed = true;
            }
            else if(characterControl.Instance._use1Input && !checkpointIsSaved) // Input.GetKeyDown(KeyCode.E)
            {
                //save checkpoint
                checkpManage.updateCheckpoint();
                checkpManage.currentCheckpointPosition = transform.position;
                checkpointIsSaved = true;
                isActivated = true;
                dataPerMan.SaveGame();
            }
            
        }
        else if(checkpointIsSaved)
        {
            checkpointIsSaved = false;
            isHealed = false;
            menuOpen = false;
            checkpMenu.SetActive(false);
        }
        
        if(characterControl.Instance.transform.position.x <= transform.position.x + 8 
        && characterControl.Instance.transform.position.x >= transform.position.x - 8
        && characterControl.Instance.transform.position.y <= transform.position.y + 8 
        && characterControl.Instance.transform.position.y >= transform.position.y - 8)
        {
            popUpSave.SetActive(true);
            if(isActivated) popUpMenu.SetActive(true);;
        }
        else 
        {
            popUpMenu.SetActive(false);
            popUpSave.SetActive(false);
        }
    }
}
