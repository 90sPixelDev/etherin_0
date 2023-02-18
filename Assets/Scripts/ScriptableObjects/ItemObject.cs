using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    CHIRO,
    RESOURCE,
    TOOL,
    WEAPON,
    ARMOR,
    CONSUMABLE
}

public abstract class ItemObject : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    public GameObject itemPrefab;
    [TextArea(5, 5)]
    public string itemDescription;

    [Space(5)]

    public int itemValue;
    public int maxStack;
    public bool isStackable { get { return (maxStack > 1); } }

    [Space(5)]

    public int maxCondition;
    public bool isDegradable {  get { return (maxCondition > -1); } }

}
