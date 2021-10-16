using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimeCount : MonoBehaviour
{
    public static TimeCount instance;

    public Text timerText;

    private TimeSpan timePlaying;
    private bool timerGoing;

    private float elapsedTime;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timerGoing = false;   
    }

    public void BeginTimer()
    {
        timerGoing = true;
        elapsedTime = 0f;

        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        timerGoing = false;
    }

    private IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingStr = "Time: " + timePlaying.ToString("mm':'ss'.'ff");
            timerText.text = timePlayingStr;

            yield return null;
        }
    }
    void Update()
    {
        
    }
}
