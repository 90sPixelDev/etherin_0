using Newtonsoft.Json.Schema;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerInventory : NetworkBehaviour
{
    [SerializeField] InventoryObject playerInventory;

    [SerializeField] GameObject playerInventorySlotsParent;

    [ClientRpc]
    public void SetInventoryReferenceClientRpc()
    {
        playerInventorySlotsParent = GameObject.FindGameObjectWithTag("PlayerInvUI").transform.GetChild(0).GetChild(0).GetChild(1).gameObject;

        InitializeInventory();
    }

    private void InitializeInventory()
    {
        for (int i = 0; i < 39; i++)
        {
            var slot = playerInventorySlotsParent.transform.GetChild(i).gameObject.GetComponentInChildren<ItemSlot>();

            if (playerInventory.Container[i] == null) continue;

            slot.item = playerInventory.Container[i];
        }
    }


}
