using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemSlot : MonoBehaviour
{
    public bool isCursor = false;

    public RectTransform slotRect;
    public Image icon;
    public TextMeshProUGUI amount;
    public Image condition;
}
