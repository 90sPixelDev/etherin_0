using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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

    public override void OnNetworkSpawn()
    {
        var networkSceneManager = NetworkManager.Singleton.SceneManager;
        NetworkManager.Singleton.SceneManager.SetClientSynchronizationMode(LoadSceneMode.Single);

        if (IsServer && !string.IsNullOrEmpty(m_SceneName))
        {
            networkSceneManager.LoadScene(m_SceneName, LoadSceneMode.Single);
            networkSceneManager.OnSceneEvent += SetupPlayer;
        }
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
        Debug.Log($"Refs are now set for Client:{client}!");
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
