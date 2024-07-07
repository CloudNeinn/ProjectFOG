using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charachterAttack : MonoBehaviour
{
    public Animator anim;
    private characterControl charCon;
    public int damageAmount = 20;
    public CapsuleCollider2D capCol;
    public bool attBool;
    private float timer;
    public charachterBlock block;
    public float radius = 1.0f;
    public LayerMask enemyLayer;
    public float capsuleWidth = 2.5f;
    public float capsuleHeight = 1.0f;
    public Vector3 centerOffset;
    public bool fastFallAttack;
    public bool specialAttackActive;

    // Start is called before the first frame update
    void Start()
    {
        charCon = GameObject.FindObjectOfType<characterControl>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(Input.GetKey(KeyCode.S))
        {
            capsuleWidth = 1.25f;
            capsuleHeight = 2f;
            centerOffset = new Vector3(0,-1.25f,0);
        }
        else if(Input.GetKey(KeyCode.W))
        {
            capsuleWidth = 1.25f;
            capsuleHeight = 2f;
            centerOffset = new Vector3(0,1f,0);                
        }
        else
        {
            capsuleWidth = 3.5f;
            capsuleHeight = 1.5f;
            centerOffset = new Vector3(0.25f * charCon.transform.localScale.x,0,0);
        }
        if (Input.GetMouseButtonDown(0) && !block.blockActive && timer >= 0.3f)
        {
            anim.SetTrigger("isAttacking");
            StartCoroutine(hitEnemies());
        }
        if(Input.GetKeyDown(KeyCode.X) && !block.blockActive && specialAttackActive)
        {
            specialAttack();
        }
    }

    public void specialAttack()
    {
        
    }

    public IEnumerator hitEnemies()
    {
        if(fastFallAttack)
        {
            centerOffset = new Vector3(0, -0.5f, 0);
            capsuleHeight = 1.5f;
            capsuleWidth = 6f;
        }
        else yield return new WaitForSeconds(0.1f);
        Collider2D[] hitEnemies = Physics2D.OverlapCapsuleAll(
            transform.position + centerOffset, 
            new Vector2(capsuleWidth, capsuleHeight), 
            capCol.direction, 
            0,
            enemyLayer
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy?.GetComponent<enemyHealth>()?.Damage(damageAmount);
        }
        timer = 0;
        fastFallAttack = false;
    }
    private void OnDrawGizmos()
    {
        // Draw a wire rectangle to represent the area
        Gizmos.color = Color.red;

        Vector3 center = transform.position + centerOffset;
        Vector3 halfExtents = new Vector3(capsuleWidth / 2, capsuleHeight / 2, 0);

        Gizmos.DrawWireCube(center, new Vector3(capsuleWidth, capsuleHeight, 0));

        // Convert CapsuleDirection2D to a Vector2
        Vector2 direction2D = new Vector2(capCol.direction == CapsuleDirection2D.Vertical ? 0 : 1, capCol.direction == CapsuleDirection2D.Vertical ? 1 : 0);

        // Draw a line representing the direction
        Gizmos.color = Color.blue;
        Vector3 startPoint = center;
        Vector3 endPoint = startPoint + new Vector3(direction2D.x, direction2D.y, 0) * (capsuleWidth / 2);
        Gizmos.DrawLine(startPoint, endPoint);
    }
}
