using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterAttack : MonoBehaviour
{
    public static characterAttack Instance;

    public Animator anim;
    public int damageAmount = 20;
    public CapsuleCollider2D capCol;
    public bool attBool;
    private float timer;
    public float radius = 1.0f;
    public LayerMask enemyLayer;
    public float capsuleWidth = 2.5f;
    public float capsuleHeight = 1.0f;
    public Vector3 centerOffset;
    public bool fastFallAttack;
    [SerializeField] public bool _isAttacking { get; private set; }
    [SerializeField] private float _originalGravity;
    [SerializeField] private float _newGravity;

    #region Gizmos
    private Vector3 center;
    private Vector3 halfExtents;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector2 direction2D;
    #endregion
    [SerializeField] private bool _comboSequence;
    [SerializeField] private float _fastComboTimer;
    [SerializeField] private float _fastComboCooldown;
    [SerializeField] private float _slowComboTimer;
    [SerializeField] private float _slowComboCooldown;

    void Awake()
    {
        Instance = this;
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        if(characterControl.Instance._moveInput.y <= -0.5f) // Input.GetKey(KeyCode.S)
        {
            capsuleWidth = 1.25f;
            capsuleHeight = 2f;
            centerOffset.Set(0,-1.25f,0);
        }
        else if(characterControl.Instance._moveInput.y >= 0.5f) // Input.GetKey(KeyCode.W)
        {
            capsuleWidth = 1.25f;
            capsuleHeight = 2f;
            centerOffset.Set(0,1f,0);                
        }
        else
        {
            capsuleWidth = 3.5f;
            capsuleHeight = 1.5f;
            centerOffset.Set(0.25f * characterControl.Instance.transform.localScale.x,0,0);
        }
        if (characterControl.Instance._attackInput && !characterBlock.Instance.blockActive && timer >= 0.3f)
        {
            anim.SetTrigger("isAttacking");
            StartCoroutine(hitEnemies());
        }
         /*
        if(_comboSequence)
        {
            _fastComboCooldown -= Time.deltaTime;
            _slowComboCooldown -= Time.deltaTime;
        }
        if(_slowComboCooldown <= 0) _comboSequence = false;
        */
        // if(Input.GetKeyDown(KeyCode.X) && !characterBlock.Instance.blockActive && specialAttackActive)
        // {
        //     specialAttack();
        // }
    }

    public void fallAttackHitEnemies()
    {
        StartCoroutine(hitEnemies());
        fastFallAttack = true;
        Debug.Log("fall attack activated");
    }

    public IEnumerator hitEnemies()
    {
        /*
        if(_fastComboCooldown > 0)
        {
            characterControl.Instance.myrigidbody.AddForce(new Vector2(2000 * Mathf.Sign(characterControl.Instance.transform.localScale.x), 200));
        }
        if(!_comboSequence)
        {
            _comboSequence = true;
            _fastComboCooldown = _fastComboTimer;
            _slowComboCooldown = _slowComboTimer;
        } 
        */
        _isAttacking = true;
        if(fastFallAttack)
        {
            centerOffset.Set(0, -0.5f, 0);
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
        if(!characterControl.Instance._isGrounded)
        {
            _originalGravity = characterControl.Instance.myrigidbody.gravityScale;
            _newGravity = 0;//_originalGravity / 28;
            characterControl.Instance.myrigidbody.velocity = Vector2.zero; //characterControl.Instance.myrigidbody.velocity / 100 ;
            characterControl.Instance.myrigidbody.gravityScale = _newGravity;
            yield return new WaitForSeconds(0.2f);
            characterControl.Instance.myrigidbody.gravityScale = _originalGravity;
        }
        _isAttacking = false;
    }
    private void OnDrawGizmos()
    {
        // Draw a wire rectangle to represent the area
        Gizmos.color = Color.red;

        center = transform.position + centerOffset;
        halfExtents.Set(capsuleWidth / 2, capsuleHeight / 2, 0);

        Gizmos.DrawWireCube(center, new Vector3(capsuleWidth, capsuleHeight, 0));

        // Convert CapsuleDirection2D to a Vector2
        direction2D.Set(capCol.direction == CapsuleDirection2D.Vertical ? 0 : 1, capCol.direction == CapsuleDirection2D.Vertical ? 1 : 0);

        // Draw a line representing the direction
        Gizmos.color = Color.blue;
        startPoint = center;
        endPoint = startPoint + new Vector3(direction2D.x, direction2D.y, 0) * (capsuleWidth / 2);
        Gizmos.DrawLine(startPoint, endPoint);
    }
}
