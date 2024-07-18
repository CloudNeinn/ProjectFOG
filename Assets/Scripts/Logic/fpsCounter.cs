using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class fpsCounter : MonoBehaviour
{
    public Text Text;
 
    private Dictionary<int, string> CachedNumberStrings = new();
    private int[] _frameRateSamples;
    private int _cacheNumbersAmount = 10000;
    private int _averageFromAmount = 30;
    private int _averageCounter = 0;
    private int _currentAveraged;

    public int lowestFps = 10000;
    private float waitForLowestTimer = 5f;
    private float waitForLowestTimerCooldown;
    public int highestFps = -1;
    public int averageFps = 0;
 
    void Awake()
    {
        waitForLowestTimerCooldown = waitForLowestTimer;
        // Cache strings and create array
        {
            for (int i = 0; i < _cacheNumbersAmount; i++) {
                CachedNumberStrings[i] = i.ToString();
            }
            _frameRateSamples = new int[_averageFromAmount];
        }
    }
    void Update()
    {
        waitForLowestTimerCooldown -= Time.unscaledDeltaTime;
        if(lowestFps > _currentAveraged && waitForLowestTimerCooldown <= 0) 
        {
            lowestFps = _currentAveraged;
            averageFps = (lowestFps + highestFps)/2;
        }
        if(highestFps < _currentAveraged) 
        {
            highestFps = _currentAveraged;
            averageFps = (lowestFps + highestFps)/2;
        }
        // Sample
        {
            var currentFrame = (int)Math.Round(1f / Time.unscaledDeltaTime); // If your game modifies Time.timeScale, use unscaledDeltaTime and smooth manually (or not).
            _frameRateSamples[_averageCounter] = currentFrame;
        }
 
        // Average
        {
            var average = 0f;
 
            foreach (var frameRate in _frameRateSamples) {
                average += frameRate;
            }
 
            _currentAveraged = (int)Math.Round(average / _averageFromAmount);
            _averageCounter = (_averageCounter + 1) % _averageFromAmount;
        }
 
        // Assign to UI
        {
            Text.text = _currentAveraged switch
            {
                var x when x >= 0 && x < _cacheNumbersAmount => CachedNumberStrings[x],
                var x when x >= _cacheNumbersAmount => $"> {_cacheNumbersAmount}",
                var x when x < 0 => "< 0",
                _ => "?"
            };
        }
    }
}