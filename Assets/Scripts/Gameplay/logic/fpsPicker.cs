using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fpsPicker : MonoBehaviour
{
    [SerializeField] private Text maxFpsText;
    [SerializeField] private int currentFpsIndex;
    [SerializeField] private int[] availableFpsSettings = {15, 30, 60, 120, -1};
    /*[SerializeField] private enum availableFpsSettings{
        [Display(Name = "15")]
        fifteen = 15,
        [Display(Name = "30")]
        thirty = 30,
        [Display(Name = "60")]
        sixty = 60,
        [Display(Name = "120")]
        hundredtwenty = 120,
        Unlimited = -1,
    }*/
    void Awake()
    {
        maxFpsText = GameObject.Find("maxFpsText").GetComponent<Text>();
        //Application.targetFrameRate = availableFpsSettings[currentFpsIndex];
        Application.targetFrameRate = availableFpsSettings[currentFpsIndex];
    }

    public void changeFps(bool forward)
    {
        if(forward) ++currentFpsIndex;
        else --currentFpsIndex;
        if(currentFpsIndex > 4) currentFpsIndex = 0;
        else if(currentFpsIndex < 0) currentFpsIndex = 4;
        Application.targetFrameRate = availableFpsSettings[currentFpsIndex];
        if(currentFpsIndex != 4) maxFpsText.text = availableFpsSettings[currentFpsIndex].ToString();
        else maxFpsText.text = "Unlimited";
    }
}
