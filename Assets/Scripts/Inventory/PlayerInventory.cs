using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : NetworkBehaviour
{
    [Header("Player inventory")]
    [SerializeField] InventoryObject playerInventory;
    [SerializeField] List<ItemSlot> playerItems;
    [SerializeField] List<UIItemSlot> playerSlots;

    [Header("GameObject References")]
    [SerializeField] Transform playerItemSlotsParent;

    [SerializeField]
    bool clientLoaded = false;

    [ClientRpc]
    public void SetInventoryReferenceClientRpc()
    {
        playerItemSlotsParent = GameObject.FindGameObjectWithTag("PlayerInvUI").transform.GetChild(0).GetChild(0).GetChild(1);

        clientLoaded = true;

        InitializeInventory();
    }

    private void InitializeInventory()
    {
        for (int i = 0; i < 39; i++)
        {
            playerSlots.Add(playerItemSlotsParent.GetChild(i).gameObject.GetComponent<UIItemSlot>());


            #region FOR TESTING
            if (i == 1)
            {
                playerItems.Add(new ItemSlot("chiro", 3852));
            }
            else
            {
                playerItems.Add(new ItemSlot());
            }
            #endregion

            //playerItems.Add(new ItemSlot());
            playerItems[i].AttachUI(playerSlots[i]);
        }
    }

    public bool AddItem(ItemObject _itemObject, int _amount, int _condition)
    {
        bool itemWasAdded = false;

        //LOOP THROUGH EVERY ITEMSLOT IN PLAYER'S INVENTORY TO CHECK FOR AVAILABILITY
        for (int i = 0; i < 39; i++)
        {
            //CHECK IF CURRENT ITEMSLOT HAS AN ITEM THE SAME AS BEING PICKED UP AND ALSO HAS ENOUGH SPACE TO ADD ANOTHER NOT PAST THE MAX STACK OF THE ITEM
            if (playerItems[i].hasItem && playerItems[i].item.maxStack >
                playerItems[i].itemAmount && playerItems[i].item == _itemObject)
            {
                Debug.Log("WorldItem added to current amount in player's inventory!");
                playerItems[i].itemAmount += _amount;
                itemWasAdded = true;
                return itemWasAdded;
            }
        }
        //IF ITEM WAS PREVIOUSLY ADDED NO NEED TO CONTINUE WITH FINDING AN EMPTY SLOT FOR THE ITEM AS IT WAS ALREADY ADDED
        if (!itemWasAdded)
        {
            for (int i = 0; i < 39; i++)
            {
                //OTHERWISE IF THERE ISN'T THE SAME ITEM IN THE INVENTORY BUT THERE IS AN EMPTY SLOT ADD THE ITEM TO THE PLAYER'S INVENTORY
                if (!playerItems[i].hasItem)
                {
                    Debug.Log("New WorldItem added!");
                    playerItems[i].item = _itemObject;
                    playerItems[i].itemCondition = _itemObject.maxCondition;
                    var amtAdded = (_amount + playerItems[i].itemAmount > playerItems[i].item.maxStack) ? playerItems[i].itemAmount = playerItems[i].item.maxStack : playerItems[i].itemAmount = _amount;
                    itemWasAdded = true;
                    return itemWasAdded;
                }
            }

            if (!itemWasAdded)
            {
                ////INDICATE INVENTORY IS FULL AND CANNOT ADD MORE ITEMS
                //gameInfoText.text = "Inventory is full!";
                //gameInfo.Play();
                //itemWasAdded = false;
                //return itemWasAdded;

                Debug.Log("INVENTORY FULL!");
            }
        }
        return itemWasAdded;
    }

    private void OnApplicationQuit()
    {
        playerInventory.Container.Clear();
    }

}
