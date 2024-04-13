using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapIconScript : MonoBehaviour
{
    private characterControl charCon;
    // Start is called before the first frame update
    void Start()
    {
        charCon = GameObject.FindObjectOfType<characterControl>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(charCon.transform.position.x,
        charCon.transform.position.y - 160, transform.position.z);
    }
}
