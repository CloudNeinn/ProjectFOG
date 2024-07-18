using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFade : MonoBehaviour
{
    public AnimationCurve FadeCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.6f, 0.7f, -1.8f, -1.2f), new Keyframe(1, 0));
    private Texture2D texture;
    private float alpha;
    private float time;
    private bool done;

    // Start is called before the first frame update
    void Start()
    {
        texture = new Texture2D(1,1);
    }

    // Update is called once per frame
    public void doFade()
    {
        done = false;
        alpha = 1;
        time = 0;
    }

    void OnGUI()
    {
        if (done) return;
        texture.SetPixel(0, 0, new Color(0, 0, 0, alpha));
        texture.Apply();
 
        time += Time.deltaTime;
        alpha = FadeCurve.Evaluate(time);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
 
        if (alpha <= 0) done = true;    
    }
}
