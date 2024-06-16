using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComputerClock : MonoBehaviour
{
    [SerializeField] private bool Use24HourTime = false;
    [SerializeField] private float SecondsBetweenUpdates = 10f;
    [SerializeField] TextMeshProUGUI timeText;

    private float timeBetweenUpdatesTimer;

    DateTime inGameTime;

    private void Update()
    {
        timeBetweenUpdatesTimer += Time.deltaTime;

        if (timeBetweenUpdatesTimer>SecondsBetweenUpdates)
        {
            timeBetweenUpdatesTimer = 0;

            inGameTime = DigitalRuby.WeatherMaker.WeatherMakerDayNightCycleManagerScript.Instance.DateTime;

            string hour = (Use24HourTime ? inGameTime.Hour : inGameTime.Hour % 12).ToString();
            string minute = inGameTime.Minute.ToString().PadLeft(2, '0');
            string amPm = "";

            if (hour == "0")
                hour = "12";

            if (!Use24HourTime)
            {
                if (inGameTime.Hour > 11)
                {
                    amPm = "PM";
                }
                else
                {
                    amPm = "AM";
                }
            }
            else
            {
                amPm = "";
            }

            timeText.text = hour + ":" + minute + " " + amPm;
        }
    }
}