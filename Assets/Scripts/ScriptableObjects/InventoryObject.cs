using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Object", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<ItemSlot> Container = new List<ItemSlot>();
}

[System.Serializable]
public class ItemSlot
{
    public ItemObject item;
    [Tooltip("UIItemSlot currently attached to")]
    [SerializeField] private UIItemSlot uiItemSlot;
    [SerializeField]
    private int _amount;
    public int itemAmount
    {
        get { return _amount; }
        set
        {
            if (item == null)
                _amount = 0;
            else if (value > item.maxStack)
                _amount = item.maxStack;
            else if (value < 1)
                _amount = 0;
            else _amount = value;
            RefreshUISlot();
        }
    }
    [SerializeField]
    private int _condition;
    public int itemCondition
    {
        get { return _condition; }
        set
        {
            if (item == null)
                _condition = 0;
            else if (value > item.maxCondition)
                _condition = item.maxCondition;
            else
                _condition = value;
            RefreshUISlot();
        }
    }

    //Quick bool to check if slot is occupied
    public bool hasItem { get { return (item != null); } }

    // PROPERTIES END

    public ItemSlot()
    {
        item = null;
        itemAmount = 0;
        itemCondition = 0;
    }
    public ItemSlot(string itemName, int _amount = 1, int _condition = 0)
    {
        ItemObject _item = FindByName(itemName); //Get our item

        if (_item == null)
        {
            item = null;
            itemAmount = 0;
            itemCondition = 0;
            return;
        }
        else
        {
            item = _item;
            itemAmount = _amount;
            itemCondition = _condition;
        }
    }

    public static ItemObject FindByName(string itemName)
    {
        itemName = itemName.ToLower(); // Make sure string is lower case (and file names too!)
        ItemObject _item = Resources.Load<ItemObject>(string.Format($"ScriptableObjects/Items/{itemName}")); // Load item from resources folder by the itemName

        if (_item == null)
            Debug.LogWarning(string.Format("Could not find \"{0}\". Item Slot is empty.", itemName));
        else
            Debug.Log($"Found {_item.name}");
        return _item;
    }

    public static bool IsSameItem(ItemSlot slotA, ItemSlot slotB)
    {
        if (slotA.item != slotB.item || slotA.itemCondition != slotB.itemCondition)
            return false;
        return true;
    }
    public static void SwapItem(ItemSlot slotA, ItemSlot slotB)
    {
        // Cache slotA's value
        ItemObject _item = slotA.item;
        int _amount = slotA.itemAmount;
        int _condition = slotA.itemCondition;

        // Copy slotB's values into slotA
        slotA.item = slotB.item;
        slotA.itemAmount = slotB.itemAmount;
        slotA.itemCondition = slotB.itemCondition;

        //Copy cached values of slotA onto slotB
        slotB.item = _item;
        slotB.itemAmount = _amount;
        slotB.itemCondition = _condition;

        // Refresh slots
        slotA.RefreshUISlot();
        slotB.RefreshUISlot();
    }

    public static void MoveItem(ItemSlot slotA, ItemSlot slotB)
    {
        // Cache slotA's value
        ItemObject _item = slotA.item;
        int _amount = slotA.itemAmount;
        int _condition = slotA.itemCondition;

        slotB.item = _item;
        slotB.itemAmount = 1;
        slotB.itemCondition = _condition;

        slotA.itemAmount -= 1;

        if (slotA.itemAmount <= 0)
        {
            slotA.item = null;
            slotA.itemCondition = 0;
            slotA.itemCondition = 0;
        }

        // Refresh slots
        slotA.RefreshUISlot();
        slotB.RefreshUISlot();
    }

    /////////////////////////////////////////////////////////////////////////////////////////

    public bool isAttachedToUI { get { return (uiItemSlot != null); } }
    public void RefreshUISlot()
    {
        // If not attached to UIInventorySlot there is nothing to refresh
        if (!isAttachedToUI)
            return;
        uiItemSlot.RefreshSlot();
    }

    public void AttachUI(UIItemSlot uiSlot)
    {
        uiItemSlot = uiSlot;
        uiItemSlot.itemSlot = this;
        RefreshUISlot();

    }

    public void DetatchUI()
    {
        uiItemSlot.ClearSlot();
        uiItemSlot = null;
    }

    public void Clear()
    {
        item = null;
        itemAmount = 0;
        itemCondition = 0;
        RefreshUISlot();
    }
}