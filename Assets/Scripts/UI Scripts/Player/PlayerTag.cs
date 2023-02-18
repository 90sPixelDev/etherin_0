using System;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerTag : NetworkBehaviour
{
    [SerializeField]
    private Transform playerCam;
    [SerializeField]
    private bool playerCamSet = false;
    [SerializeField]
    private TextMeshProUGUI playerTagTMPro;
    [SerializeField]
    NetworkVariable<FixedString128Bytes> playerTag = new NetworkVariable<FixedString128Bytes>();

    public override void OnNetworkSpawn()
    {
        playerTag.OnValueChanged += OnPlayerTagChanged;
    }

    private void OnPlayerTagChanged(FixedString128Bytes previousValue, FixedString128Bytes newValue)
    {
        SetPlayerTag(newValue.ToString());
    }

    private void Start()
    {
        playerTagTMPro = GetComponentInChildren<TextMeshProUGUI>();
    }

    [ClientRpc]
    public void SetPlayerCamRefClientRpc()
    {
        Debug.Log($"Running SetPlayerCamRef on Billboard!");
        playerCam = GameObject.FindGameObjectWithTag("FPCam").transform;

        SetPlayerTagColor();
        UpdatePlayerTagVarServerRpc();

        playerCamSet = true;
        if (IsLocalPlayer) transform.GetChild(0).gameObject.SetActive(false);
    }

    public void SetPlayerTag(string newVal = "default")
    {
        Debug.Log("SETTING TAG!");
        playerTagTMPro.text = playerTag.Value.ToString();
    }

    public void InitializePlayerTag()
    {
        Debug.Log("Initializing Player Tag!");
        playerTag.Value = $"Player: {GetComponentInParent<NetworkObject>().OwnerClientId + 1}";
    }

    [ServerRpc(RequireOwnership=false)]
    public void UpdatePlayerTagVarServerRpc()
    {
        Debug.Log("1st SET Player Tag!");
        playerTag.Value = $"Player: {GetComponentInParent<NetworkObject>().OwnerClientId + 1}";
    }

    [ServerRpc(RequireOwnership=false)]
    public void SetPlayerTagInMenuServerRpc()
    {
        Debug.Log("Changing Player Tag!");
        playerTag.Value = $"<sprite name=menu> Player: {GetComponentInParent<NetworkObject>().OwnerClientId + 1}";
    }

    private void SetPlayerTagColor()
    {
        var ownerId = GetComponentInParent<NetworkObject>().OwnerClientId;
        switch (ownerId)
        {
            case 0:
                playerTagTMPro.color = Color.red;
                break;
            case 1:
                playerTagTMPro.color = Color.blue;
                break;
            case 2:
                playerTagTMPro.color = Color.green;
                break;
            case 3:
                playerTagTMPro.color = Color.yellow;
                break;
            case 4:
                playerTagTMPro.color = new Color(235, 52, 222);
                break;
            case 5:
                playerTagTMPro.color = new Color(235, 140, 52);
                break;
            case 6:
                playerTagTMPro.color = new Color(134, 52, 235);
                break;
            case 7:
                playerTagTMPro.color = new Color(183, 235, 52);
                break;
        }
    }

    private void LateUpdate()
    {
        if (playerCamSet)
        {
            transform.LookAt(transform.position + playerCam.rotation * Vector3.forward, playerCam.rotation * Vector3.up);
        }
    }
}
