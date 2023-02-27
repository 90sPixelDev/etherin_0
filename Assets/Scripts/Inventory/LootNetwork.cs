using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class LootNetwork : NetworkBehaviour
{
    [Header("Loot Info")]
    public string lootName;
    public bool isRandom;
    public bool isLooted;
    [Header("Loot Items")]
    public InventoryObject inventoryItems;
    public List<ItemSlot> lootItems = new List<ItemSlot>();
    //[Header("Player Info")]
    //public NetworkList<ulong> PlayersWhoLooted = new NetworkList<ulong>();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            foreach (ulong networkCLientID in NetworkManager.Singleton.ConnectedClientsIds)
            {
                InitializeLootClientRpc(networkCLientID);
            }

            NetworkManager.Singleton.OnClientConnectedCallback += InitializeLootClientRpc;
        }
    }
    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= InitializeLootClientRpc;

        }
    }

    [ClientRpc]
    private void InitializeLootClientRpc(ulong obj)
    {
        if (isRandom)
        {
            ItemObject[] tempItems = new ItemObject[3];
            tempItems[0] = Resources.Load<ItemObject>("ScriptableObjects/Items/basic_axe_01");
            tempItems[1] = Resources.Load<ItemObject>("ScriptableObjects/Items/chiro");
            tempItems[2] = Resources.Load<ItemObject>("ScriptableObjects/Items/wood_r");

            int randomLootSize = Random.Range(1, 6);

            for (int i = 0; i < 32; i++)
            {
                if (i <= randomLootSize)
                {
                    int index = Random.Range(0, 3);
                    int amount = Random.Range(1, tempItems[index].maxStack);
                    int condition = Random.Range(0, tempItems[index].maxCondition);

                    lootItems.Add(new ItemSlot(tempItems[index].name, amount, condition));
                }

                if (i > randomLootSize)
                    lootItems.Add(new ItemSlot());
            }
        }
        else
        {
            foreach (ItemSlot item in inventoryItems.Container)
            {
                lootItems.Add(item);
            }
        }
    }
}
