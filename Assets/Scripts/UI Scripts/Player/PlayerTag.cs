using TMPro;
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
    NetworkVariable<NetworkString> m_playerTag = new NetworkVariable<NetworkString>();

    public override void OnNetworkSpawn()
    {
        m_playerTag.OnValueChanged += OnPlayerTagChanged;
    }

    private void OnPlayerTagChanged(NetworkString previousValue, NetworkString newValue)
    {
        SetPlayerTag();
    }
    public void SetPlayerTag()
    {
        playerTagTMPro.text = m_playerTag.Value;
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
        SetPlayerTag();

        playerCamSet = true;
    }

    [ServerRpc(RequireOwnership=false)]
    public void UpdatePlayerTagVarServerRpc()
    {
        m_playerTag.Value = $"Player: {OwnerClientId + 1}";
    }

    [ServerRpc(RequireOwnership=false)]
    public void SetPlayerTagInMenuServerRpc()
    {
        m_playerTag.Value = $"<sprite name=menu> Player: {OwnerClientId + 1}";
    }

    private void SetPlayerTagColor()
    {
        var ownerId = OwnerClientId;
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
