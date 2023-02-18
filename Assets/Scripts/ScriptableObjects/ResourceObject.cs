using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    THATCH,
    WOOD,
    STONE,
}

[CreateAssetMenu(fileName = "New Resource Object", menuName = "Inventory System/Items/Resource")]
public class ResourceObject : ItemObject
{
    public ResourceType resourceType;

    public void Awake()
    {
        itemType = ItemType.RESOURCE;
        maxCondition = -1;
    }
}
