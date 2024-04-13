using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    private float length, startPosition;
    public GameObject camera;
    public float effectStrength;

    void Start()
    {
        startPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float distance = (camera.transform.position.x * effectStrength);
        float temp = (camera.transform.position.x * (1 - effectStrength));
        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);
        if(temp > startPosition + length) startPosition += length;
        else if(temp < startPosition - length) startPosition -= length;
    }
}