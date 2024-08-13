using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHealthManager : MonoBehaviour
{
    public static playerHealthManager Instance;
    public Image healthBar;
    public float healthAmount;
    public float maxHealth;
    public GameObject player;
    public Rigidbody2D charRigid;
    public enemyHealth enemy;
    public bool canMove;
    public float knockTimer;
    public float invincibilityTimer;
    public bool playerDead;
    public bool isDamageable;
    public bool barrierActive;
    public float barrierAmount;
    public float maxBarrier;
    public Image barrierBar;
    public bool barrierHasTimer;
    public float barrierTimer;
    public float barrierTimeCounter;
    public float barrierCooldownTimer;
    public float barrierCooldownCounter;
    private Vector2 force;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        isDamageable = true;
        charRigid = characterControl.Instance.gameObject.GetComponent<Rigidbody2D>(); 
        healthAmount = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Barrier();
        //for testing purposes-----------
            /*if(Input.GetKeyUp(KeyCode.L))
            {
                getDamage(20);
            }
            
            if(Input.GetKeyUp(KeyCode.K))
            {
                Heal(10);
            }*/
        //-------------------------------
    }
        
    public void getDamage(float damage, bool stopTime = true)
    {   
        if(isDamageable && !barrierActive)
        {
            //canMove = false;
            StartCoroutine(slowTimeInvincible(stopTime));
            healthAmount -= damage;

            healthBar.fillAmount = healthAmount / maxHealth;
            if(healthAmount <= 0)
            {
                // you died scene
                playerDead = true;
                charRigid.bodyType = RigidbodyType2D.Static;
            }
            
        }
        else if(isDamageable && barrierActive)
        {
            barrierAmount -= damage;
            //barrierBar.fillAmount = barrierAmount / maxBarrier;
            if(barrierAmount <= 0) barrierActive = false;
        }
    }

    public void getDamage(float damage, float knockbackStrengthX, float knockbackStrengthY, bool stopTime = true)
    {   
        if(isDamageable && !barrierActive)
        {
            canMove = false;
            StartCoroutine(slowTimeInvincible(stopTime));
            healthAmount -= damage;
            if(enemy != null)
            {
                charRigid.velocity = new Vector2(0, charRigid.velocity.y);
                force.Set(knockbackStrengthX * -enemy.attackDirection(), knockbackStrengthY);
                charRigid.AddForce(force);
            }
            StartCoroutine(knockBackTime());
            //charRigid.velocity = new Vector2(knockStrengthX * -enemy.attackDirection(), knockStrengthY);
            //charRigid.velocity = Vector2.zero * 0;
            //charRigid.velocity = Vector2.right * -enemy.attackDirection() * knockStrengthX + Vector2.up * knockStrengthY;

            healthBar.fillAmount = healthAmount / maxHealth;
            if(healthAmount <= 0)
            {
                // you died scene
                playerDead = true;
                charRigid.bodyType = RigidbodyType2D.Static;
            }
            
        }
        else if(isDamageable && barrierActive)
        {
            barrierAmount -= damage;
            //barrierBar.fillAmount = barrierAmount / maxBarrier;
            if(barrierAmount <= 0) barrierActive = false;
        }
    }

    public void Heal(float amount)
    {
        healthAmount += amount;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth);
        healthBar.fillAmount = healthAmount / maxHealth;
    }

    public void Barrier()
    {
        if(!barrierActive && barrierCooldownCounter > 0) barrierCooldownCounter -= Time.deltaTime;
        if(barrierActive)
        {
            barrierTimeCounter -= Time.deltaTime;
            if(barrierTimeCounter <= 0) 
            {
                barrierAmount = 0;
                barrierActive = false;
            }
        }
        if(characterControl.Instance.hasBarrier && Input.GetKeyDown(KeyCode.B) && !barrierActive && barrierCooldownCounter <= 0)
        {
            barrierActive = true;
            barrierCooldownCounter = barrierCooldownTimer;
            barrierAmount = maxBarrier;
            barrierTimeCounter = barrierTimer;
        }
    }
    IEnumerator slowTimeInvincible(bool stopTime)
    {
        isDamageable = false;
        if(stopTime) Time.timeScale = 0.6f;
        yield return new WaitForSeconds(invincibilityTimer);
        if(stopTime) Time.timeScale = 1f;
        isDamageable = true;
        
    }
    IEnumerator knockBackTime()
    {
        yield return new WaitForSeconds(knockTimer);
        characterControl.Instance.anim.SetBool("isMoving", false);
        canMove = true;
    }
}
