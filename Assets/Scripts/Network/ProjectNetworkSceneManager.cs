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
        var networkSceneManager = NetworkManager.Singleton.SceneManager;
        NetworkManager.Singleton.SceneManager.SetClientSynchronizationMode(LoadSceneMode.Single);

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(GetUserPublicIP(), (ushort)27007, ipInputField.text);

        if (IsServer && !string.IsNullOrEmpty(m_SceneName))
        {
            networkSceneManager.LoadScene(m_SceneName, LoadSceneMode.Single);
            networkSceneManager.OnSceneEvent += SetupPlayer;
        }
    }

    public override void OnNetworkDespawn()
    {
        var networkSceneManager = NetworkManager.Singleton.SceneManager;
        networkSceneManager.OnSceneEvent -= SetupPlayer;
    }

    private string GetUserPublicIP()
    {
        string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
        var externalIp = IPAddress.Parse(externalIpString);

        Debug.Log($"Starting server with IP of:{externalIp}");
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
        Debug.Log("Setting References!");
        var playerGO = NetworkManager.Singleton.ConnectedClients[client].PlayerObject;

        ClientRpcParams clientRpcParams = new()
        {
           Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { client } }
        };

        playerGO.GetComponent<CharacterControllerScript>().SetReferencesClientRPC(clientRpcParams);
        playerGO.GetComponent<PlayerNetworkVitals>().SetVitalsReferencesClientRPC(clientRpcParams);
        playerGO.GetComponentInChildren<Billboard>().SetPlayerCamRefClientRpc();

        Debug.Log($"Refs are now set for Client:{client}!");
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
