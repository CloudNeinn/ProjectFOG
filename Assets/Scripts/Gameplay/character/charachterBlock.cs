using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charachterBlock : MonoBehaviour
{
    public BoxCollider2D blockCollider;
    public bool blockActive;
    public Animator anim;
    private characterControl charCon;
    // Start is called before the first frame update
    void Start()
    {
        charCon = transform.parent.gameObject.GetComponent<characterControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (charCon._blockInput) // Input.GetMouseButtonDown(1)
        {
            blockActive = true;
            blockCollider.enabled = true;
            anim.SetBool("isBlocking", true);
        }
        else // if (!charCon._blockInput) // Input.GetMouseButtonUp(1)
        {
            blockActive = false;
            blockCollider.enabled = false;
            anim.SetBool("isBlocking", false);
        }
    }
}
