using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class disappearingWall : MonoBehaviour
{
    public Tilemap tilemap;
    public Color color;
    private bool fadeOut = false;
    private float fadeSpeed = 6f;
    // Start is called before the first frame update
    void Start()
    {
        color = GetComponent<Tilemap>().color;
        tilemap = GetComponent<Tilemap>();
        color.a = 1;
        tilemap.color = color;
    }

    // Update is called once per frame
    
    void Update()
    { 
        
        if(fadeOut)
        {
            color.a = Mathf.Lerp(0, 1f, color.a -= fadeSpeed * Time.deltaTime);
            //color.a -= fadeSpeed * Time.deltaTime;
            tilemap.color = color;
        }
        else
        {
            color.a = Mathf.Lerp(0, 1f, color.a += fadeSpeed * Time.deltaTime);
            //color.a += fadeSpeed * Time.deltaTime;
            tilemap.color = color;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            fadeOut = true; //Destroy(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            fadeOut = false; //Destroy(gameObject);
        }
    }
}
