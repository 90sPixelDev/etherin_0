using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClockTime : MonoBehaviour
{

    public Image clock;
    public TextMeshProUGUI gameHour;
    public TextMeshProUGUI gameMinute;
    public TextMeshProUGUI gameDay;
    [Tooltip("In Realtime Seconds!")]
    public float clockCompletionTime;

    public int hour;
    public int minute;

    private DayNightCycle dayNightCycle;

    // Start is called before the first frame update
    void Start()
    {
        dayNightCycle = GameObject.Find("DayNightCycle").GetComponent<DayNightCycle>();
        clockCompletionTime = (dayNightCycle.targetDayLength * 60);
    }

    // Update is called once per frame
    void Update()
    {
        ClockMovement();
        CalculateGameTime();
        UpdateGameDay();
    }

    public void ClockMovement()
    {
        clock.fillAmount -= 1 / clockCompletionTime * Time.deltaTime;
        if (clock.fillAmount == 0)
        {
            clock.fillAmount = 1;
        }
        if (dayNightCycle.isNight)
            clock.color = new Color(0.7f, 0.7f, 1f, 0.7f);
        else
            clock.color = new Color(1f, 1f, 0.7f, 0.7f);
    }

    public void CalculateGameTime()
    {
        hour = (int)(dayNightCycle.timeOfDay / 0.083333333f); // 12 full count on clock (This is for 10 second day only)
        minute = (int)(dayNightCycle.timeOfDay / 0.001388f ) % 60; // 720 full count on clock (This is for 10 second day only)

        if (hour == 0)
            hour = 12;
        if (minute == 0)
            minute = 60;

        gameHour.text = hour.ToString();
        gameMinute.text = minute.ToString();
    }

    public void UpdateGameDay()
    {
        gameDay.text = string.Format("Day: {0}", dayNightCycle.dayNumber.ToString());
    }
}
