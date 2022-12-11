using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerVitals : MonoBehaviour
{
    //All my damn variables for this code
    [Header("Health")]
    public float maxHealth;
    public float currentHealth;
    public TextMeshProUGUI healthPercentText;
    [Header("Stamina")]
    public float maxStamina;
    public float currentStamina;
    public TextMeshProUGUI staminaPercentText;
    [Header("Food")]
    public float maxFood;
    public float currentFood;
    public TextMeshProUGUI hungerPercent;
    [Header("Water")]
    public float maxWater;
    public float currentWater;
    public TextMeshProUGUI thirstPercent;
    [Space(5)]
    public Image staminaBar;
    public Image healthBar;
    public Image foodBar;
    public Image waterBar;
    [Header("Adjustment for Size of vital Bars")]
    public float barNumberAdjustment = 3.28f;
    public float needsBarNumberAdjustment = 0.83f;

    public PlayerMovement playerMovementScript;


    // Start is called before the first frame update
    void Start()
    {
        //Finding the healthBarUI
        healthBar = GameObject.Find("CurrentHealthBar").GetComponent<Image>();
        staminaBar = GameObject.Find("CurrentStaminaBar").GetComponent<Image>();
        healthPercentText = GameObject.Find("HealthPercentText").GetComponent<TextMeshProUGUI>();
        staminaPercentText = GameObject.Find("StaminaPercentText").GetComponent<TextMeshProUGUI>();

        playerMovementScript = GetComponent<PlayerMovement>();

        float healthPercent = 1f;
        float staminaPercent = 1f;

        //Setting max health
        maxHealth = 100f;
        //Setting current health to max health in case of errors
        currentHealth = maxHealth;
        //Setting the health percent text to the current health of player at start
        healthPercentText.text = healthPercent.ToString("P");
        //Setting max Stamina
        maxStamina = 100f;
        //Setting current Stamina to max Stamina in case of errors
        currentStamina = maxStamina;
        //Setting the stamina percent text to the current stamina of player at start
        staminaPercentText.text = staminaPercent.ToString("P");
        //Convert the actual health to bar width
        float barWidth = currentHealth * barNumberAdjustment;
        //Setting the size of the healthbar so no issues from start
        healthBar.rectTransform.sizeDelta = new Vector2(barWidth, healthBar.rectTransform.sizeDelta.y);
        //Setting the color of the health bar correctly from start
        Color fullHealthColor = new Color(0.502f, 0f, 0.0112f, 1f);
        healthBar.color = fullHealthColor;

        //setting max Food
        currentFood = 100f;
        float needsBarAdj = currentFood * needsBarNumberAdjustment;
        foodBar.rectTransform.sizeDelta = new Vector2(foodBar.rectTransform.sizeDelta.x, needsBarAdj);

    }

    //Function to update the health depending on damage received or healing taken
    public void RefreshHealth(float amt)
    {
        //Add the amount that should be healed or damaged
        currentHealth += amt;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        if (currentHealth < 0f)
            currentHealth = 0f;
        //Convert the actual health to bar width
        float barWidth = currentHealth * barNumberAdjustment;
        //Update the height of the health bar to current health
        healthBar.rectTransform.sizeDelta = new Vector2(barWidth, healthBar.rectTransform.sizeDelta.y);
        //Updating the health percent text to current health
        float healthPercent = currentHealth / maxHealth;
        healthPercentText.text = healthPercent.ToString("P");
    }

    public void RefreshStamina()
    {
        if (currentStamina > 0f && Input.GetKey(KeyCode.LeftShift))
            currentStamina -= 10f * Time.deltaTime;
        else
            currentStamina += 10f * Time.deltaTime;
        if (currentStamina > 100f)
            currentStamina = 100f;
        if (currentStamina < 0f)
        {
            currentStamina = 0f;
        }
        staminaBar.rectTransform.sizeDelta = new Vector2(currentStamina * barNumberAdjustment, staminaBar.rectTransform.sizeDelta.y);
        float staminaPercent = currentStamina / maxStamina;
        staminaPercentText.text = staminaPercent.ToString("P");
    }

    public void RefreshFood()
    {
        float needsBarAdj = currentFood * needsBarNumberAdjustment;

        if (currentFood > 0f)
        {
            currentFood -= .03f * Time.deltaTime;

            //Making sure that when player runs their food/water also goes down quicker
            if (playerMovementScript.isRunning)
                currentFood -= 0.05f * Time.deltaTime;
        }
        if (currentFood < 0f)
        {
            currentFood = 0f;
        }
        foodBar.rectTransform.sizeDelta = new Vector2(foodBar.rectTransform.sizeDelta.x, needsBarAdj);
        hungerPercent.text = currentFood.ToString("#00.0") + "%";

    }
    public void RefreshWater()
    {
        float needsBarAdj = currentWater * needsBarNumberAdjustment;

        if (currentWater > 0f)
        {
            currentWater -= 0.05f * Time.deltaTime;

            //Making sure that when player runs their food/water also goes down quicker
            if (playerMovementScript.isRunning)
                currentWater -= 0.075f * Time.deltaTime;
        }

        if (currentWater < 0f)
        {
            currentWater = 0f;
        }
        waterBar.rectTransform.sizeDelta = new Vector2(waterBar.rectTransform.sizeDelta.x, needsBarAdj);
        thirstPercent.text = currentWater.ToString("#00.0") + "%";
    }

    public void PlayerDeath()
    {
        //Make the rest of the function to kill character when health reaches 0
    }

    // Update is called once per frame
    void Update()
    {
        RefreshStamina();
        RefreshFood();
        RefreshWater();

        if (Input.GetKeyDown(KeyCode.UpArrow))
            RefreshHealth(5f);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            RefreshHealth(-5f);

        //Setting up the variable float for something between 0 and 1 to change colors depending on current health the player has
        float healthPercent = currentHealth / maxHealth;

        //Setting up the colors
        Color fullHealthColor = new Color(0.502f, 0f, 0.0112f, 1f);
        Color lowHealthColor = new Color(0.7452f, 0.4042f, 0.4122f, 1f);
        //The actual code to lerp the colors from one to the other depending on the percent
        healthBar.color = Color.Lerp(lowHealthColor, fullHealthColor, healthPercent);
    }
}
