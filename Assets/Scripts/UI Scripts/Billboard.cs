using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Billboard : NetworkBehaviour
{
    [SerializeField]
    private Transform playerCam;
    [SerializeField]
    private bool playerCamSet = false;
    [SerializeField]
    private TextMeshProUGUI playerTagTMPro;

    private void Start()
    {
        playerTagTMPro = GetComponentInChildren<TextMeshProUGUI>();
    }

    [ClientRpc]
    public void SetPlayerCamRefClientRpc()
    {
        Debug.Log("Running SetPlayerCamRef on Billboard!");
        playerCam = GameObject.FindGameObjectWithTag("FPCam").transform;
        playerTagTMPro.text = $"Player: {gameObject.GetComponentInParent<NetworkObject>().OwnerClientId + 1}";
        SetPlayerTagColor();

        playerCamSet = true;
    }

    private void SetPlayerTagColor()
    {
        var ownerId = gameObject.GetComponentInParent<NetworkObject>().OwnerClientId;
        switch  (ownerId)
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
