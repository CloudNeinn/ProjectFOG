using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charachterBlock : MonoBehaviour
{
    public BoxCollider2D blockCollider;
    public bool blockActive;
    public Animator anim;

    void Update()
    {
        if (characterControl.Instance._blockInput) // Input.GetMouseButtonDown(1)
        {
            blockActive = true;
            blockCollider.enabled = true;
            anim.SetBool("isBlocking", true);
        }
        else // if (!characterControl.Instance._blockInput) // Input.GetMouseButtonUp(1)
        {
            blockActive = false;
            blockCollider.enabled = false;
            anim.SetBool("isBlocking", false);
        }
    }
}
