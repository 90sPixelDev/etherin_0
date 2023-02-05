using UnityEngine;
using UnityEngine.UI;
using TMPro;

//[ExecuteInEditMode]
public class ClockTime : MonoBehaviour
{
    public bool isTimeRunning = false;
    public Image hand;
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
    private void Start()
    {
        dayNightCycle = GameObject.Find("DayNightCycleNew").GetComponent<DayNightCycle>();
        clockCompletionTime = (dayNightCycle.targetDayLength * 60);
        isTimeRunning = true;
    }
    void Update()
    {
        if (isTimeRunning)
        {
            ClockMovement();
            CalculateGameTime();
            UpdateGameDay();
        }
    }

    public void ClockMovement()
    {
        var handZDegree = 0f;
        handZDegree -= 30f * dayNightCycle.timeOfDay;
        hand.transform.localEulerAngles = new Vector3(0, 0, handZDegree);
        if (handZDegree == 360f)
        {
            handZDegree = 0f;
        }
        if (dayNightCycle.isNight)
            clock.color = new Color(0.7f, 0.7f, 1f, 0.7f);
        else
            clock.color = new Color(1f, 1f, 0.7f, 0.7f);
    }

    public void CalculateGameTime()
    {
        hour = (int)(dayNightCycle.timeOfDay);
        minute = (int)((dayNightCycle.timeOfDay - Mathf.Floor(dayNightCycle.timeOfDay)) * 60);

        if (hour == 0)
            hour = 24;
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
