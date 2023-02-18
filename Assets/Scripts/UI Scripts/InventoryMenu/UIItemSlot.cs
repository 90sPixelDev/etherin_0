using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    //public bool isCursor = false;

    //public RectTransform slotRect;
    //public Image icon;
    //public TextMeshProUGUI amount;
    //public Image condition;

    public ItemSlot itemSlot;
    public ItemSlot pointerItemSlot;

    [SerializeField] Button btn;

    private void OnEnable()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => SwitchItemSlots(pointerItemSlot, itemSlot));
    }

    private void Start()
    {
        itemSlot = GetComponentInChildren<ItemSlot>();
        pointerItemSlot = GameObject.FindGameObjectWithTag("MouseContainer").GetComponent<ItemSlot>();
    }

    public void SwitchItemSlots(ItemSlot itemSlot_a, ItemSlot itemSlot_b)
    {
        Debug.Log("Switching!");

        itemSlot_a.item = itemSlot_b.item;
        itemSlot_a.UpdateItemInfo();
    }


}
