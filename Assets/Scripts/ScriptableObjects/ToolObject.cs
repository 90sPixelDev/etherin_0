using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolType
{
    AXE,
    PICKAXE,
}

[CreateAssetMenu(fileName = "New Tool Object", menuName = "Inventory System/Items/Tool")]
public class ToolObject : ItemObject
{
    public ToolType toolType;

    public void Awake()
    {
        itemType = ItemType.TOOL;
    }
}
