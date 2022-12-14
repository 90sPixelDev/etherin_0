using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenEffects : MonoBehaviour
{
    public PlayerVitals PlayerVitals;

    public Image hurtBG;

    Color noAlpha = new Color(1f, 1f, 1f, 1f);
    Color fullAlpha = new Color(1f, 1f, 1f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        //PlayerVitals = GameObject.Find("FPSPlayer").GetComponent<PlayerVitals>();
        //hurtBG = GameObject.Find("HurtEffect").GetComponent<Image>();
        //hurtBG.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (PlayerVitals.currentHealth <= 50)
        //{
        //    hurtBG.enabled = true;
        //    float alphaValue = (PlayerVitals.currentHealth * 2.5f) / 100f;
        //    hurtBG.color = Color.Lerp(noAlpha, fullAlpha, alphaValue);
        //}
        //else if (PlayerVitals.currentHealth > 50)
        //    hurtBG.enabled = false;
    }
}
