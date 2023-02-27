using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LootContainerNetwork : NetworkBehaviour
{
    [SerializeField]
    List<UIItemSlot> uiSlots = new List<UIItemSlot>();
    [SerializeField]
    PlayerNetworkState playerNetworkState;
    [SerializeField]
    SelectionManager selectionManager;
    [SerializeField]
    CharacterControllerScript characterControllerScript;

    [Header("Loot References")]
    [SerializeField]
    WorldItem currWorldItem;
    [SerializeField]
    LootNetwork loot;

    [Header("UI References")]
    [SerializeField]
    public TextMeshProUGUI lootTitle;
    [SerializeField]
    GameObject lootWindowParent;
    [SerializeField]
    Transform contentWindow;
    [SerializeField]
    GameObject slotPrefab;

    void Start()
    {
        slotPrefab = Resources.Load<GameObject>("Prefabs/UI/Inventory/UIItemSlot");
        playerNetworkState = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerNetworkState>();
    }

    [ClientRpc]
    public void SetLootReferencesClientRpc()
    {
        lootWindowParent = GameObject.FindGameObjectWithTag("LootUI");
        lootTitle = lootWindowParent.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        contentWindow = lootWindowParent.transform.GetChild(0).GetChild(0).GetChild(1).GetComponentInChildren<GridLayoutGroup>().transform;
        selectionManager = GameObject.FindGameObjectWithTag("FPCam").GetComponent<SelectionManager>();
        characterControllerScript = GetComponentInParent<CharacterControllerScript>();
    }

    [ServerRpc]
    public void CommandLootUIServerRpc(ulong clientID)
    {
        Debug.Log("Got this far!");

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientID }
            }
        };

        ToggleLootUIClientRpc(clientRpcParams);
    }

    [ClientRpc]
    private void ToggleLootUIClientRpc(ClientRpcParams clientRpcParams)
    {
        if (!IsOwner) return;

        if (playerNetworkState.n_inLootMenu.Value)
        {
            characterControllerScript.InventoryUI();
            CloseLoot();
        }
        else
        {
            characterControllerScript.InventoryUI();
            OpenLoot();
        }
    }

    private void OpenLoot()
    {
        lootWindowParent.transform.GetChild(0).gameObject.SetActive(true);
        loot = selectionManager.GetComponent<SelectionManager>().selectable.GetComponent<LootNetwork>();
;
        lootTitle.text = loot.lootName;

        // Loop through each item and instantiate UI Element for it!
        for (int i = 0; i < loot.lootItems.Count; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, contentWindow);

            newSlot.name = i.ToString();

            uiSlots.Add(newSlot.GetComponent<UIItemSlot>());

            loot.lootItems[i].AttachUI(uiSlots[i]);
        }
        playerNetworkState.n_inLootMenu.Value = true;
    }

    public void CloseLoot()
    {
        // Loop through each slot and dettach as well as delete
        foreach (UIItemSlot slot in uiSlots)
        {
            slot.itemSlot.DetatchUI();
            Destroy(slot.gameObject);
        }
        uiSlots.Clear();
        lootWindowParent.transform.GetChild(0).gameObject.SetActive(false);
        playerNetworkState.n_inLootMenu.Value = false;
    }
}