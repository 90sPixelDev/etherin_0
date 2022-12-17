using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectSceneManager : NetworkBehaviour
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

    public override void OnNetworkSpawn()
    {
        if (IsServer && !string.IsNullOrEmpty(m_SceneName))
        {
            var status = NetworkManager.SceneManager.LoadScene(m_SceneName, LoadSceneMode.Additive);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load {m_SceneName} " +
                      $"with a {nameof(SceneEventProgressStatus)}: {status}");
            }
            NetworkManager.Singleton.SceneManager.OnSceneEvent += SceneManager_OnSceneEvent;
        }
    }

    private void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
    {
        switch (sceneEvent.SceneEventType)
        {
            case SceneEventType.LoadComplete:
                {
                    // This will let you know when a load is completed
                    // Server Side: receives this notification for both itself and all clients
                    if (IsServer)
                    {
                        if (sceneEvent.ClientId == NetworkManager.LocalClientId)
                        {
                            // Handle server side LoadComplete related tasks here
                            SceneManager.SetActiveScene(sceneEvent.Scene);
                            var playerNGO = NetworkManager.Singleton.ConnectedClients[sceneEvent.ClientId].PlayerObject;
                            SceneManager.MoveGameObjectToScene(playerNGO.gameObject, sceneEvent.Scene);
                            playerNGO.GetComponent<CharacterControllerScript>().SetReferences();
                            playerNGO.gameObject.transform.position = new Vector3(0, 40, 0);
                            SceneManager.UnloadSceneAsync(0);
                        }
                        else
                        {
                            // Handle client LoadComplete **server-side** notifications here
                        }
                    }
                    else if (IsClient &&  sceneEvent.ClientId == NetworkManager.LocalClientId) // Clients generate this notification locally
                    {
                        Debug.Log("Client unloading scene!");
                        
                    }

                    // So you can use sceneEvent.ClientId to also track when clients are finished loading a scene
                    break;
                }
        }
    }
}
