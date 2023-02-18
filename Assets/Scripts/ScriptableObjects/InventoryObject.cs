using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Object", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<ItemObject> Container = new List<ItemObject>();

    public bool hasItem(ItemObject item)
    {
        bool doesHaveItem = false;
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].name == item.name)
            {
                Debug.Log($"Has {item.name}");
                doesHaveItem = true;
                break;
            }
        }

        return doesHaveItem;
    }
}


