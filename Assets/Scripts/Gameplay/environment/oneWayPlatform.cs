using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class oneWayPlatform : MonoBehaviour
{

    public GameObject currentOneWayPlatform;
    public GameObject colliderForEnemy;
    public characterControl charCon;
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("player"), LayerMask.NameToLayer("oneWayForEnemy"), true);
        charCon = gameObject.GetComponent<characterControl>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            if (currentOneWayPlatform != null) StartCoroutine(DisableCollision());
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }
    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformColliderBox = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        if(platformColliderBox == null) 
        {
            CompositeCollider2D platformColliderTile = currentOneWayPlatform.GetComponent<CompositeCollider2D>();
            Physics2D.IgnoreCollision(charCon.coll, platformColliderTile);
            Physics2D.IgnoreCollision(charCon.collCrouch, platformColliderTile);
            yield return new WaitForSeconds(0.25f);
            Physics2D.IgnoreCollision(charCon.coll, platformColliderTile, false);
            Physics2D.IgnoreCollision(charCon.collCrouch, platformColliderTile, false);
        }
        else
        {
            Physics2D.IgnoreCollision(charCon.coll, platformColliderBox);
            Physics2D.IgnoreCollision(charCon.collCrouch, platformColliderBox);
            yield return new WaitForSeconds(0.25f);
            Physics2D.IgnoreCollision(charCon.coll, platformColliderBox, false);
            Physics2D.IgnoreCollision(charCon.collCrouch, platformColliderBox, false);
        }
    }
}
