using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerExperience : MonoBehaviour
{
    [Header("Levels")]
    public int currentLevel;
    public int maxLevel;
    [Header("EXP")]
    public float earnedEXP;
    public float maxEXP = 660f;

    public float barWidth;

    public Image earnedEXPBar;
    public TextMeshProUGUI levelText;

    public Animation levelUpTextAnim;
    public Animation levelUpGlowL;
    public Animation levelUpGlowR;

    Color startColor = new Color(0.191f, 0.000f, 0.681f, 1.000f);
    Color endColor = new Color(0.7579f, 0.0967f, 0.8207f, 1.000f);

    // Start is called before the first frame update
    void Start()
    {
        earnedEXPBar = GameObject.Find("FilledExpBar").GetComponent<Image>();

        float currentColor = (float)barWidth / (float)maxEXP;

        earnedEXP = 170f;
        barWidth = earnedEXP;

        earnedEXPBar.rectTransform.sizeDelta = new Vector2(barWidth, earnedEXPBar.rectTransform.sizeDelta.y);
        earnedEXPBar.color = Color.Lerp(startColor, endColor, currentColor);

        currentLevel = 1;
        levelText.text = currentLevel.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            EarnEXP(100f);
        }
    
    }

    public void EarnEXP(float earned)
    {
        barWidth += earned;

        //Code to make when adding exp to not go over bar width limit of 660 here!
            if (barWidth >= 660f)
        {
            LevelUp();
            barWidth -= 660;
            earnedEXP -= 660f;

            earnedEXPBar.rectTransform.sizeDelta = new Vector2(barWidth, earnedEXPBar.rectTransform.sizeDelta.y);
        }

        float currentColor = (float)barWidth / (float)maxEXP;

        earnedEXPBar.color = Color.Lerp(startColor, endColor, currentColor);
        earnedEXPBar.rectTransform.sizeDelta = new Vector2(barWidth, earnedEXPBar.rectTransform.sizeDelta.y);
    }

    public void LevelUp()
    {
        currentLevel++;
        levelText.text = currentLevel.ToString();
        levelUpTextAnim.Play();
        levelUpGlowL.Play();
        levelUpGlowR.Play();
    }
}
