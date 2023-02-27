using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectNetworkSceneManager : NetworkBehaviour
{
#if UNITY_EDITOR
    public UnityEditor.SceneAsset SceneAsset;
    private void OnValidate()
    {
        if (SceneAsset != null)
        {
            m_SceneName = SceneAsset.name;
        }
    }
#endif
    [SerializeField]
    private string m_SceneName;
    [SerializeField]
    private ClockTime clockTime;
    [SerializeField]
    private PlayerNetworkManager playerNetworkManager;
    [SerializeField]
    private TMP_InputField ipInputField;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        var netManager = NetworkManager.Singleton;
        var networkSceneManager = netManager.SceneManager;
        NetworkManager.Singleton.SceneManager.SetClientSynchronizationMode(LoadSceneMode.Single);

        if (IsServer && !IsHost)
        {
            Debug.Log($"Starting Server with IP of:{netManager.GetComponent<UnityTransport>().ConnectionData.Address}");
        }
        else if (IsHost)
        {
            Debug.Log($"Starting Server with IP of:{netManager.GetComponent<UnityTransport>().ConnectionData.Address}");
            Debug.Log($"Server IP:{netManager.GetComponent<UnityTransport>().ConnectionData.Address}");
        }
        else if (IsClient && !IsHost)
        {
            Debug.Log($"Connecting to Server with IP of: {netManager.GetComponent<UnityTransport>().ConnectionData.ServerListenAddress}");
            Debug.Log($"Server IP:{netManager.GetComponent<UnityTransport>().ConnectionData.Address}");
        }

        if (IsServer && !string.IsNullOrEmpty(m_SceneName))
        {
            networkSceneManager.LoadScene(m_SceneName, LoadSceneMode.Single);
            networkSceneManager.OnSceneEvent += SetupPlayer;
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        var networkSceneManager = NetworkManager.Singleton.SceneManager;
        networkSceneManager.OnSceneEvent -= SetupPlayer;
    }

    public void UpdateListenIP(string newText)
    {
        ipInputField.text = newText;
    }

    public void SetNetworkConfig()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(GetUserPublicIP(), (ushort)27007, ipInputField.text);
    }

    private string GetUserPublicIP()
    {
        string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
        var externalIp = IPAddress.Parse(externalIpString);

        return externalIp.ToString();
    }

    private void SetupPlayer(SceneEvent sceneEvent)
    {
        if (sceneEvent.SceneName == m_SceneName && sceneEvent.SceneEventType == SceneEventType.LoadComplete)
        {
            foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                SetReferences(client.ClientId);
            }
        }
    }

    private void SetReferences(ulong client)
    {
        var playerGO = NetworkManager.Singleton.ConnectedClients[client].PlayerObject;

        ClientRpcParams clientRpcParams = new()
        {
           Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { client } }
        };

        playerGO.GetComponent<CharacterControllerScript>().SetReferencesClientRPC(clientRpcParams);
        playerGO.GetComponent<PlayerNetworkVitals>().SetVitalsReferencesClientRPC(clientRpcParams);
        playerGO.GetComponentInChildren<PlayerInventory>().SetInventoryReferenceClientRpc();
        playerGO.GetComponentInChildren<PlayerTag>().SetPlayerCamRefClientRpc();
        playerGO.GetComponentInChildren<LootContainerNetwork>().SetLootReferencesClientRpc();
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
