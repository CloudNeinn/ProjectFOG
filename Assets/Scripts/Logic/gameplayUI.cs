using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class gameplayUI : MonoBehaviour
{
    //public playerHealthManager pHM;
    public GameObject pauseMenu;
    public GameObject pauseButton;
    public GameObject deathScreen;
    public GameObject settingsMenu;
    public GameObject checkpointMenu;
    public GameObject CHB;
    public Rigidbody2D charRigid;
    public DataPersistanceManager dataPerMan;
    public enemyBehaviour[] eneBehs;
    public cameraFade camFade;
    private spawnManager spawnMan;
    private GameObject map;
    // Start is called before the first frame update
    void Start()
    {
        //pHM  = GameObject.FindObjectOfType<playerHealthManager>();
        charRigid = characterControl.Instance.gameObject.GetComponent<Rigidbody2D>();
        dataPerMan = GameObject.FindObjectOfType<DataPersistanceManager>();
        camFade = GameObject.FindObjectOfType<cameraFade>();
        spawnMan = GameObject.FindObjectOfType<spawnManager>();
        pauseMenu = GameObject.Find("Pause Menu");
        settingsMenu =  GameObject.Find("Settings Menu");
        checkpointMenu =  GameObject.Find("CheckpointMenu");
        pauseButton = GameObject.Find("Pause button");
        deathScreen =  GameObject.Find("Death screen");
        CHB =  GameObject.Find("CharachterHealthBar");
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        deathScreen.SetActive(false);
        map = GameObject.Find("Map");
        map.SetActive(false);
        checkpointMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealthManager.Instance.playerDead)
        {
            deathScreenMenu();
        }
        if(pauseMenu.activeSelf && Input.GetKeyDown(KeyCode.Escape) && !(playerHealthManager.Instance.playerDead))
        {
            turnOffPause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !(playerHealthManager.Instance.playerDead) 
        && !(map.activeSelf) && !(checkpointMenu.activeSelf))
        {
            Pause();
        }
        if(checkpointMenu.activeSelf && 
        Input.GetKeyDown(KeyCode.Escape)) 
        {
            checkpointMenu.SetActive(false);
        }
        if(settingsMenu.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            settingsMenu.SetActive(false);
            pauseMenu.SetActive(true);   
        }
        triggerMap();

    }

    public void Pause()
    {
        CHB.SetActive(false);
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0f;
    }

    public void deathScreenMenu()
    {
        deathScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void turnOffPause()
    {
        CHB.SetActive(true);
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
    }

    public void Respawn()
    {
        DataManager.Instance.hasToRespawn = true;
        eneBehs = GameObject.FindObjectsOfType<enemyBehaviour>();
        camFade.doFade();
        DataPersistanceManager.Instance.LoadGame();
        playerHealthManager.Instance.playerDead = false;
        if(spawnMan != null) spawnMan.Spawn();
        if(eneBehs != null)
        {
            foreach(enemyBehaviour eneBeh in eneBehs)
            {
                eneBeh.target = eneBeh.enemyStartingPosition;
                eneBeh.transform.position = eneBeh.enemyStartingPosition;
            }
        }
        //characterControl.Instance.transform.position = checkpManage.currentCheckpointPosition;
        playerHealthManager.Instance.healthAmount = playerHealthManager.Instance.maxHealth;
        charRigid.bodyType = RigidbodyType2D.Dynamic;
        deathScreen.SetActive(false);
        playerHealthManager.Instance.healthBar.fillAmount = playerHealthManager.Instance.healthAmount / playerHealthManager.Instance.maxHealth;
        turnOffPause();
    }

    public void SettingsInGame()
    {
        if(!settingsMenu.activeSelf)
        {
            settingsMenu.SetActive(true);
            pauseMenu.SetActive(false);
        }
        else if(settingsMenu.activeSelf)
        {
            settingsMenu.SetActive(false);
            pauseMenu.SetActive(true);          
        }
    }

    public void triggerMap()
    {
        if(Input.GetKeyDown(KeyCode.M) && !(map.activeSelf))
        {
            CHB.SetActive(false);
            pauseButton.SetActive(false);
            Time.timeScale = 0f;
            map.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.M) && map.activeSelf || 
        Input.GetKeyDown(KeyCode.Escape) && map.activeSelf)
        {
            CHB.SetActive(true);
            pauseButton.SetActive(true);
            Time.timeScale = 1f;
            map.SetActive(false);
        }
    }
}
