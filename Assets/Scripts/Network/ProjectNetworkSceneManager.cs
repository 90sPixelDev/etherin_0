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
    private NetworkObject playerCam;
    [SerializeField]
    private NetworkObject menuUI;
    [SerializeField]
    private NetworkObjectReference[] playerRefs;
    [SerializeField]
    private bool refsLoaded = false;

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
            if (!refsLoaded)
            {
                playerCam = GameObject.FindGameObjectWithTag("FPCam").GetComponent<NetworkObject>();
                menuUI = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<NetworkObject>();
                refsLoaded = true;
            }

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

        if (playerGO.GetComponent<CharacterControllerScript>().refsLoaded)
        {
            Debug.Log("Refs are already Loaded!");
        }
        else
        {
            playerGO.GetComponent<CharacterControllerScript>().SetReferencesClientRPC();
            //playerGO.GetComponent<UnityEngine.InputSystem.PlayerInput>().enabled = true;
            Debug.Log($"Refs are now set for {client}!");
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
