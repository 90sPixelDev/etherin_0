using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{

    [Header("WorldItem Info")]
    public ItemSlot itemSlot;

    [Header("WorldItem Info References")]
    public TextMeshProUGUI amtText;
    public Image conditionBar;

    [Header("UI Refs")]
    public Image icon;
    public RectTransform slotRect;
    public Button slotButton;

    [Header("Pointer")]
    public bool isCursor = false;
    public InventorySlotControls invClickHandler;

    private void OnEnable()
    {
        invClickHandler = GameObject.FindGameObjectWithTag("AllPlayerUI").GetComponent<InventorySlotControls>();

        if (isCursor) return;
        slotButton = GetComponent<Button>();
        slotButton.onClick.AddListener(() => invClickHandler.ProcessClick(this));
    }
    private void OnDisable()
    {
        slotButton.onClick.RemoveAllListeners();
    }


    //private void Awake()
    //{
        //itemSlot = new ItemSlot();
        //itemSlot.AttachUI(this);
    //}

    // VISUAL UPDATES
    public void RefreshSlot()
    {
        UpdateAmount();
        UpdateIcon();
        UpdateConditionBar();
    }
    public void ClearSlot()
    {
        itemSlot = null;
        RefreshSlot();
    }


    public void UpdateIcon()
    {
        if (itemSlot == null || !itemSlot.hasItem)
            icon.enabled = false;
        else
        {
            icon.enabled = true;
            icon.sprite = itemSlot.item.itemIcon;
        }
    }

    public void UpdateAmount()
    {
        if (itemSlot == null || !itemSlot.hasItem || itemSlot.itemAmount < 2)
            amtText.enabled = false;
        else
        {
            amtText.enabled = true;
            amtText.text = itemSlot.itemAmount.ToString();
        }
    }

    private void UpdateConditionBar()
    {
        if (itemSlot == null || !itemSlot.hasItem || !itemSlot.item.isDegradable)
            conditionBar.enabled = false;
        else
        {
            conditionBar.enabled = true;

            //Get normalized percentage
            float conditionPercent = (float)itemSlot.itemCondition / (float)itemSlot.item.maxCondition;

            float barWidth = slotRect.rect.width * conditionPercent;

            conditionBar.rectTransform.sizeDelta = new Vector2(barWidth - 15, conditionBar.rectTransform.sizeDelta.y);

            //Lerp condition bar from red to green (Change to be tier based in the future)
            conditionBar.color = Color.Lerp(Color.red, Color.green, conditionPercent);
        }

    }


}
