using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerOptionsConfig : MonoBehaviour
{
    [Header("Config Values")]
    public int mouseXSensitivity = 15;
    public int mouseYSensitivity = 15;
    public int gamepadXSensitivity = 170;
    public int gamepadYSensitivity = 170;

    [Header("References")]
    [SerializeField]
    private TextMeshProUGUI mouseXSenText;
    [SerializeField]
    private TextMeshProUGUI mouseYSenText;
    [SerializeField]
    private TextMeshProUGUI gamepadXSenText;
    [SerializeField]
    private TextMeshProUGUI gamepadYSenText;
    [Space(5)]
    [SerializeField]
    private Slider mouseXSenSlider;
    [SerializeField]
    private Slider mouseYSenSlider;
    [SerializeField]
    private Slider gamepadXSenSlider;
    [SerializeField]
    private Slider gamepadYSenSlider;

    private void Awake()
    {
        if (!mouseXSenText || mouseXSenText == null)
        {
            mouseXSenText = GameObject.Find("MouseXCon").transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        }
        if (!mouseYSenText || mouseYSenText == null)
        {
            mouseYSenText = GameObject.Find("MouseYCon").transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        }
        if (!gamepadXSenText || gamepadXSenText == null)
        {
            gamepadXSenText = GameObject.Find("GamePadXCon").transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        }
        if (!gamepadYSenText || gamepadYSenText == null)
        {
            gamepadYSenText = GameObject.Find("GamePadYCon").transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        }

    }

    private void Start()
    {
        mouseXSenText.text = mouseXSensitivity.ToString();
        mouseYSenText.text = mouseYSensitivity.ToString();
        gamepadXSenText.text = gamepadXSensitivity.ToString();
        gamepadYSenText.text = gamepadYSensitivity.ToString();
    }

    public void OnMouseXChange()
    {
        mouseXSensitivity = (int)mouseXSenSlider.value;
        mouseXSenText.text = mouseXSensitivity.ToString();
    }
    public void OnMouseYChange()
    {
        mouseYSensitivity = (int)mouseYSenSlider.value;
        mouseYSenText.text = mouseYSensitivity.ToString();

    }
    public void OnGamePadXChange()
    {
        gamepadXSensitivity = (int)gamepadXSenSlider.value;
        gamepadXSenText.text = gamepadXSensitivity.ToString();
    }
    public void OnGamePadYChange()
    {
        gamepadYSensitivity = (int)gamepadYSenSlider.value;
        gamepadYSenText.text = gamepadYSensitivity.ToString();
    }
}
