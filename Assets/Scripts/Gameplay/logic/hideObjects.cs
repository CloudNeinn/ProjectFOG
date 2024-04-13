using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideObjects : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.tag == "Respawn")
        {
            /*
            for(int i = 0; i < collider.transform.childCount; i++)
            {
                collider.transform.GetChild(i).gameObject.SetActive(true);
            }
            */
            foreach(Transform child in collider.transform)
            {
                child.transform.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.transform.tag == "Respawn")
        {
            /*
            for(int i = 0; i < collider.transform.childCount; i++)
            {
                collider.transform.GetChild(i).gameObject.SetActive(false);
            }
            */
            foreach(Transform child in collider.transform)
            {
                child.transform.gameObject.SetActive(false);
            }
        }
    }
}
