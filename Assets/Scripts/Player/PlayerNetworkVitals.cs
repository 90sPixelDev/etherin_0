using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class PlayerNetworkVitals : NetworkBehaviour
{
    [Header("Health")]
    public NetworkVariable<float> n_MaxHealth = new NetworkVariable<float>(100f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> n_Health = new NetworkVariable<float>(100f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public TextMeshProUGUI healthPercentText;
    [Header("Stamina")]
    public NetworkVariable<float> n_MaxStamina = new NetworkVariable<float>(100f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    //public NetworkVariable<float> currentHealth;
    public NetworkVariable<float> n_CurrentStamina = new NetworkVariable<float>(100f,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public TextMeshProUGUI staminaPercentText;
    [Header("Food")]
    public NetworkVariable<float> maxFood;
    public NetworkVariable<float> currentFood;
    public TextMeshProUGUI hungerPercent;
    [Header("Water")]
    public NetworkVariable<float> maxWater;
    public NetworkVariable<float> currentWater;
    public TextMeshProUGUI thirstPercent;
    [Space(5)]
    public Image staminaBar;
    public Image healthBar;
    public Image foodBar;
    public Image waterBar;
    [Header("Adjustment for Size of vital Bars")]
    public float barNumberAdjustment = 3.28f;
    public float needsBarNumberAdjustment = 0.83f;

    public CharacterControllerScript characterControllerScript;
    public bool refsLoaded = false;


    // Start is called before the first frame update
    void Start()
    {
        characterControllerScript = GetComponent<CharacterControllerScript>();

        ////Setting max Stamina
        //maxStamina = 100f;
        ////Setting current Stamina to max Stamina in case of errors
        //currentStamina = maxStamina;
        ////Setting the stamina percent text to the current stamina of player at start
        //staminaPercentText.text = staminaPercent.ToString("P");
        ////Convert the actual health to bar width
        //float barWidth = currentHealth * barNumberAdjustment;
        ////Setting the size of the healthbar so no issues from start
        //healthBar.rectTransform.sizeDelta = new Vector2(barWidth, healthBar.rectTransform.sizeDelta.y);
        ////Setting the color of the health bar correctly from start
        //Color fullHealthColor = new Color(0.502f, 0f, 0.0112f, 1f);
        //healthBar.color = fullHealthColor;

        ////setting max Food
        //currentFood = 100f;
        //float needsBarAdj = currentFood * needsBarNumberAdjustment;
        //foodBar.rectTransform.sizeDelta = new Vector2(foodBar.rectTransform.sizeDelta.x, needsBarAdj);

    }

    private void SetVitalsDefault()
    {
        float healthPercent = n_Health.Value / n_MaxHealth.Value;
        healthPercentText.text = healthPercent.ToString("P");
    }

    [ClientRpc]
    public void SetVitalsReferencesClientRPC(ClientRpcParams clientRpcParams)
    {
        healthBar = GameObject.FindGameObjectWithTag("CurrentHealthBar").GetComponent<Image>();
        staminaBar = GameObject.FindGameObjectWithTag("CurrentStaminaBar").GetComponent<Image>();
        healthPercentText = GameObject.FindGameObjectWithTag("HealthPercentText").GetComponent<TextMeshProUGUI>();
        staminaPercentText = GameObject.FindGameObjectWithTag("StaminaPercentText").GetComponent<TextMeshProUGUI>();
        SetVitalsDefault();
    }

    //Function to update the health depending on damage received or healing taken
    public void EditHealth(float amt)
    {
        Debug.Log($"Health changed by {amt}");
        //Add the amount that should be healed or damaged
        n_Health.Value += amt;
        if (n_Health.Value > n_MaxHealth.Value)
            n_Health.Value = n_MaxHealth.Value;
        if (n_Health.Value < 0f)
            n_Health.Value = 0f;
        //Convert the actual health to bar width
        float barWidth = n_Health.Value * barNumberAdjustment;
        //Update the height of the health bar to current health
        healthBar.rectTransform.sizeDelta = new Vector2(barWidth, healthBar.rectTransform.sizeDelta.y);
        //Updating the health percent text to current health
        float healthPercent = n_Health.Value / n_MaxHealth.Value;
        healthPercentText.text = healthPercent.ToString("P");
    }

    //public void RefreshStamina()
    //{
    //    if (currentStamina > 0f && Input.GetKey(KeyCode.LeftShift))
    //        currentStamina -= 10f * Time.deltaTime;
    //    else
    //        currentStamina += 10f * Time.deltaTime;
    //    if (currentStamina > 100f)
    //        currentStamina = 100f;
    //    if (currentStamina < 0f)
    //    {
    //        currentStamina = 0f;
    //    }
    //    staminaBar.rectTransform.sizeDelta = new Vector2(currentStamina * barNumberAdjustment, staminaBar.rectTransform.sizeDelta.y);
    //    float staminaPercent = currentStamina / maxStamina;
    //    staminaPercentText.text = staminaPercent.ToString("P");
    //}

    //public void RefreshFood()
    //{
    //    float needsBarAdj = currentFood * needsBarNumberAdjustment;

    //    if (currentFood > 0f)
    //    {
    //        currentFood -= .03f * Time.deltaTime;

    //        //Making sure that when player runs their food/water also goes down quicker
    //        if (characterControllerScript.GetisRunning)
    //            currentFood -= 0.05f * Time.deltaTime;
    //    }
    //    if (currentFood < 0f)
    //    {
    //        currentFood = 0f;
    //    }
    //    foodBar.rectTransform.sizeDelta = new Vector2(foodBar.rectTransform.sizeDelta.x, needsBarAdj);
    //    hungerPercent.text = currentFood.ToString("#00.0") + "%";

    //}
    //public void RefreshWater()
    //{
    //    float needsBarAdj = currentWater * needsBarNumberAdjustment;

    //    if (currentWater > 0f)
    //    {
    //        currentWater -= 0.05f * Time.deltaTime;

    //        //Making sure that when player runs their food/water also goes down quicker
    //        if (characterControllerScript.GetisRunning)
    //            currentWater -= 0.075f * Time.deltaTime;
    //    }

    //    if (currentWater < 0f)
    //    {
    //        currentWater = 0f;
    //    }
    //    waterBar.rectTransform.sizeDelta = new Vector2(waterBar.rectTransform.sizeDelta.x, needsBarAdj);
    //    thirstPercent.text = currentWater.ToString("#00.0") + "%";
    //}

    public void PlayerDeath()
    {
        //Make the rest of the function to kill character when health reaches 0
    }

    // Update is called once per frame
    void Update()
    {
        //RefreshStamina();
        //RefreshFood();
        //RefreshWater();

        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //    RefreshHealth(5f);
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //    RefreshHealth(-5f);

        ////Setting up the variable float for something between 0 and 1 to change colors depending on current health the player has
        //float healthPercent = currentHealth / maxHealth;

        ////Setting up the colors
        //Color fullHealthColor = new Color(0.502f, 0f, 0.0112f, 1f);
        //Color lowHealthColor = new Color(0.7452f, 0.4042f, 0.4122f, 1f);
        ////The actual code to lerp the colors from one to the other depending on the percent
        //healthBar.color = Color.Lerp(lowHealthColor, fullHealthColor, healthPercent);
    }
}
