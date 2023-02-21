using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Chiro Object", menuName ="Inventory System/Items/Chiro")]
public class ChiroObject : ItemObject
{
    public void Awake()
    {
        itemType = ItemType.CHIRO;
        itemName = "Chiro";
        itemValue = 1;
        maxCondition = -1;
    }
}
