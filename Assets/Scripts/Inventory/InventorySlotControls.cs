using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventorySlotControls : NetworkBehaviour
{
    [Header("UI")]
    public UIItemSlot cursor;

    [Header("Event Info")]
    public EventSystem eventSystem;
    public PointerEventData pointer;
    public GraphicRaycaster raycaster;

    [Header("Player Controls")]
    private string controlScheme;

    public bool isCarryingItem { get { return (cursor.itemSlot.hasItem); } }

    public void Awake()
    {
        raycaster = GetComponent<GraphicRaycaster>();

        eventSystem = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<EventSystem>();
    }

    public void ProcessClick(UIItemSlot pressedUISlot)
    {
        if (pressedUISlot.itemSlot.hasItem)
            Debug.Log(pressedUISlot.itemSlot.item.itemName);

        // Catch null errors
        if (pressedUISlot == null)
        {
            Debug.LogWarning("UI Element tagged as UIInventorySlot but has no UIInventorySlot component");
            return;
        }
        // If Inventory Slots are differet, we just swap them
        if (!ItemSlot.IsSameItem(cursor.itemSlot, pressedUISlot.itemSlot))
        {
            ItemSlot.SwapItem(cursor.itemSlot, pressedUISlot.itemSlot);
            cursor.RefreshSlot();
            pressedUISlot.RefreshSlot();

            if (cursor.itemSlot.hasItem && pressedUISlot.itemSlot.hasItem)
                Debug.Log(string.Format("Swapped \"{0}\" with \"{1}\"!", pressedUISlot.itemSlot.item.itemName, cursor.itemSlot.item.itemName));
            return;
        }
        // Instead if items are identical ...
        if (ItemSlot.IsSameItem(cursor.itemSlot, pressedUISlot.itemSlot))
        {
            // If the slots are empty just return
            if (!cursor.itemSlot.hasItem)
            {
                Debug.Log("The slots are empty, nothing to swap or do here!");
                return;
            }
            // If the slot is not stackable also return
            if (!cursor.itemSlot.item.isStackable)
            {
                Debug.Log("The slot is not stackable!");
                return;
            }
            // If the slot is fully stacked also return
            if (pressedUISlot.itemSlot.itemAmount == pressedUISlot.itemSlot.item.maxStack)
            {
                Debug.Log("The slot is fully stacked!");
                return;
            }

            // Otherwise we add up the amounts and put as much as possible into slot, leaving rest on cursor
            int total = cursor.itemSlot.itemAmount + pressedUISlot.itemSlot.itemAmount;
            int maxStack = cursor.itemSlot.item.maxStack; // Cache the max stack

            // If what cursor is holding and what is in pressedUISlot slot are less than the max possible stack than add the cursor
            // items over to the slot and remove the cursor item
            if (total <= maxStack)
            {
                pressedUISlot.itemSlot.itemAmount = total;
                cursor.itemSlot.Clear();
            }
            // If the cursor holds more of the item than the slot can hold then move over the items from cursor that can be moved
            // and leave the rest in cursor hanging
            else
            {
                pressedUISlot.itemSlot.itemAmount = maxStack;
                cursor.itemSlot.itemAmount = total - maxStack;
            }
            cursor.RefreshSlot();
            pressedUISlot.RefreshSlot();
        }
    }
}
