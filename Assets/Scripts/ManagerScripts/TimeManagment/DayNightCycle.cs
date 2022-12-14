using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DayNightCycle : MonoBehaviour
{
    [Header("Time")]
    [Tooltip("Day Length in Minutes")]
    [SerializeField]
    private float _targetDayLength = .5f; // Length of day in minutes
    public float targetDayLength
    {
        get
        {
            return _targetDayLength;
        }
    }

    [SerializeField]
    [Range(0f, 24f)]
    private float _timeOfDay;
    public float timeOfDay
    {
        get
        {
            return _timeOfDay;
        }
    }

    [SerializeField]
    private int _dayNumber = 1;
    public int dayNumber
    {
        get
        {
            return _dayNumber;
        }
    }

    [SerializeField]
    private int _yearNumber = 0;
    public int yearNumber
    {
        get
        {
            return _yearNumber;
        }
    }

    private float _timeScale = 100f;
    [SerializeField]
    private int _yearLength = 100;
    public int yearLength
    {
        get
        {
            return _yearLength;
        }
    }
    public bool pause = false;

    [Header("Sun Light")]
    [SerializeField]
    private Transform sunRotation;
    private Transform moonRotation;
    private Light sun;
    [SerializeField] private float intensity;
    [SerializeField]
    private float SunBaseIntensity = 1f;
    [SerializeField]
    private float sunVariation = 1.5f;
    [SerializeField]
    private Gradient sunColor;
    [SerializeField]
    private Gradient directionalColor;
    [SerializeField]
    private Gradient fogColor;

    [Header("Seasonal Variables")]
    [SerializeField]
    private Transform sunSeasonalRotation;
    [SerializeField]
    [Range(-45f, 45f)]
    private float maxSeasonalTilt;

    public bool isNight { get { return (timeOfDay > 0.8f || timeOfDay < 0.2); } }


    private void Start()
    {
        sunSeasonalRotation = GameObject.Find("Seasonal Rotation").GetComponent<Transform>();
        sunRotation = GameObject.Find("Sun Rotation").GetComponent<Transform>();
        moonRotation = GameObject.Find("Moon Rotation").GetComponent<Transform>();
        sun = GameObject.Find("Sun").GetComponent<Light>();
    }

    private void Update()
    {
        if (!pause)
        {
            UpdateTimeScale();
            UpdateTime();
        }

        AdjustSunRotation(timeOfDay / 24f);
        AdjustSunColor(timeOfDay / 24f);
        AdjustAmbientColor(timeOfDay / 24f);
        AdjustFogColor(timeOfDay / 24f);
        SunIntensity();

        AdjustMoonRotation(timeOfDay / 24f);
    }

    private void UpdateTimeScale()
    {
        _timeScale = 24f / (_targetDayLength / 60f);
    }

    private void UpdateTime()
    {
        _timeOfDay += Time.deltaTime * _timeScale / 3600; //Seconds in an hour
        if (_timeOfDay > 24) //Day completed!
        {
            _dayNumber++;
            _timeOfDay -= 24;

            if (_dayNumber > _yearLength) //Year has been completed!
            {
                _yearNumber++;
                _dayNumber = 1;
            }
        }
    }

    //Rotates the sun daily (and seasonally soon too)
    private void AdjustSunRotation(float timePercent)
    {
        float sunAngle = timePercent * 360f;
        sunRotation.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, sunAngle));

        float seasonalAngle = -maxSeasonalTilt * Mathf.Cos((float) dayNumber / (float) _yearLength * 2f * Mathf.PI);
        sunSeasonalRotation.localRotation = Quaternion.Euler(new Vector3(seasonalAngle, 0f, 0f));
    }
    private void AdjustMoonRotation(float timePercent)
    {
        float moonAngle = (timePercent + 180f) * 360f;

        moonRotation.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, moonAngle));
    }

    private void SunIntensity()
    {
        intensity = Vector3.Dot(sun.transform.forward, Vector3.down);
        intensity = Mathf.Clamp01(intensity);

        sun.intensity = intensity * sunVariation + SunBaseIntensity;
    }

    private void AdjustSunColor(float timePercent)
    {
        sun.color = directionalColor.Evaluate(timePercent);
    }

    private void AdjustAmbientColor(float timePercent)
    {
        RenderSettings.ambientLight = sunColor.Evaluate(timePercent);
    }
    private void AdjustFogColor(float timePercent)
    {
        RenderSettings.fogColor = fogColor.Evaluate(timePercent);
    }
}
