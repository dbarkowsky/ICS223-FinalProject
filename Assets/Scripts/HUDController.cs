using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timer;
    private float timePassed = 0; // time in seconds
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        timer.text = "Time: " + CalculateTime();
    }

    private String CalculateTime()
    {
        float minutes = Mathf.FloorToInt(timePassed / 60);
        float seconds = Mathf.FloorToInt(timePassed % 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}
