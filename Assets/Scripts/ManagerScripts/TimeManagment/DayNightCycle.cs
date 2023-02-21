using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

//[ExecuteInEditMode]
public class DayNightCycle : NetworkBehaviour
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
    private NetworkVariable<float> _timeOfDay = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone);
    public float timeOfDay
    {
        get
        {
            return _timeOfDay.Value;
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
    public bool pauseTime = false;

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

    public bool isNight { get { return (timeOfDay < 8f || timeOfDay > 20); } }


    private void Start()
    {
        if (IsServer)
        {
            sunSeasonalRotation = GameObject.Find("Seasonal Rotation").GetComponent<Transform>();
            sunRotation = GameObject.Find("Sun Rotation").GetComponent<Transform>();
            moonRotation = GameObject.Find("Moon Rotation").GetComponent<Transform>();
            sun = GameObject.Find("Sun").GetComponent<Light>();

            // Temp Set of time for Debugging/Testing
            _timeOfDay.Value = 13f;
        }
    }

    private void Update()
    {
        if (!pauseTime && IsServer)
        {
            UpdateTimeScale();
            UpdateTime();

            AdjustSunRotation(timeOfDay / 24f);
            AdjustSunColor(timeOfDay / 24f);
            AdjustAmbientColor(timeOfDay / 24f);
            AdjustFogColor(timeOfDay / 24f);
            SunIntensity();

            AdjustMoonRotation(timeOfDay / 24f);
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            sunSeasonalRotation = GameObject.Find("Seasonal Rotation").GetComponent<Transform>();
            sunRotation = GameObject.Find("Sun Rotation").GetComponent<Transform>();
            moonRotation = GameObject.Find("Moon Rotation").GetComponent<Transform>();
            sun = GameObject.Find("Sun").GetComponent<Light>();

            _timeOfDay.OnValueChanged += UpdateTimeForClients;
        }
    }
    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            _timeOfDay.OnValueChanged -= UpdateTimeForClients;
        }
    }

    private void UpdateTimeScale()
    {
        _timeScale = 24f / (_targetDayLength / 60f);
    }

    private void UpdateTime()
    {
        _timeOfDay.Value += Time.deltaTime * _timeScale / 3600; //Seconds in an hour
        if (_timeOfDay.Value > 24) //Day completed!
        {
            _dayNumber++;
            _timeOfDay.Value -= 24;

            if (_dayNumber > _yearLength) //Year has been completed!
            {
                _yearNumber++;
                _dayNumber = 1;
            }
        }
    }

    private void UpdateTimeForClients(float previousValue, float newValue)
    {
        AdjustSunRotation(newValue / 24f);
        AdjustSunColor(newValue / 24f);
        AdjustAmbientColor(newValue / 24f);
        AdjustFogColor(newValue / 24f);
        SunIntensity();

        AdjustMoonRotation(newValue / 24f);
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
