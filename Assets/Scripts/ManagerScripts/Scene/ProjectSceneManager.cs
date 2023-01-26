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
        if ((IsServer || IsHost) && !string.IsNullOrEmpty(m_SceneName))
        {
            NetworkManager.Singleton.SceneManager.LoadScene(m_SceneName, LoadSceneMode.Single);
        }
    }
}
