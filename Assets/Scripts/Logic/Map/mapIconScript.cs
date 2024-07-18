using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapIconScript : MonoBehaviour
{
    void Update()
    {
        transform.position = new Vector3(characterControl.Instance.transform.position.x,
        characterControl.Instance.transform.position.y - 160, transform.position.z);
    }
}
