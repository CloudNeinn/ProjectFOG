using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{    
    [SerializeField] private string id;

    [ContextMenu("Generate enemy id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    [Header ("Health parameters")]
    public bool vulnerable;
    public int maxHealth;
    public int currentHealth;
    public bool attacked;
    public bool isAlive = true;

    [Header ("Timer parameters")]
    public float invulnerabilityTime = 0.6f;
    public float vulTimer;

    [Header ("References")]
    public Rigidbody2D enemyRigidBody;
    public enemyPatrol enePat;
    public enemyVision eneVis;
    public enemyBehaviour eneBeh;
    public playerHealthManager pHM;

    public float knockbackStrengthX;
    public float knockbackStrengthY;
    public Vector2 knockbackForce;
    public float attackedTime;
    private int numberOfBloodPoints;
    [SerializeField] private GameObject _blood;
    private ObjectPooling objectPooling;
    // Start is called before the first frame update
    void Start()
    {
        enePat = gameObject.GetComponent<enemyPatrol>();
        eneVis = gameObject.GetComponent<enemyVision>();
        eneBeh = gameObject.GetComponent<enemyBehaviour>();
        objectPooling = gameObject.GetComponent<ObjectPooling>();
        pHM = GameObject.FindObjectOfType<playerHealthManager>();
        vulTimer = 0;
        vulnerable = true;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //vulTimer += Time.deltaTime;
        //if(enePat.hasToPatrol) knockbackStrength = 10;
        //else knockbackStrength = 3;
    }
   
    public virtual void Damage(int amount)
    {
        if(currentHealth > 0 && vulnerable)
        {   
            numberOfBloodPoints = Random.Range(2, 4);
            vulnerable = false;
            currentHealth -= amount;
            attacked = true;
            knockbackForce.Set(knockbackStrengthX * attackDirection(), knockbackStrengthY); // used to be new Vector2
            enemyRigidBody.AddForce(knockbackForce);
            //enemyRigidBody.velocity = new Vector2(knockbackStrengthX * attackDirection(), knockbackStrengthY);
            //enemyRigidBody.AddForce(new Vector2(200 * attackDirection(), 150), ForceMode2D.Impulse);    
            if (currentHealth <= 0)
            {
                numberOfBloodPoints = Random.Range(3, 6);
                isAlive = false;
                if(enePat != null) enePat.enemiesSpawnPoint.isAliveSpawner = false;
                Debug.Log("EnemyDied");
                Destroy(gameObject);
            }
            
            for(int i = 0; i < numberOfBloodPoints; i++)
            {
                ObjectPoolManager.SpawnObject(_blood, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.BloodCoinObjects);
                // GameObject blood = objectPooling.GetPooledObject();
                // if(blood != null)
                // {
                //     blood.transform.position = transform.position;
                //     blood.SetActive(true);
                // }
                // Instantiate(_blood, transform.position, Quaternion.identity);
            }
            
            while(vulTimer <= invulnerabilityTime)
            {
                vulTimer += Time.deltaTime;
            }
            vulnerable = true;
            vulTimer = 0;
            StartCoroutine(attackedCooldown());
        }
    }
    
    public int attackDirection()
    {
        if(characterControl.Instance.transform.position.x < transform.position.x) return 1;
        else return -1;
    }

    public IEnumerator attackedCooldown()
    {
        yield return new WaitForSeconds(attackedTime);
        attacked = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //pHM.enemy = GetComponent<enemyHealth>();
        }
    }

}
