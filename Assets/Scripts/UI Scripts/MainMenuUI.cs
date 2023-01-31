using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private GameObject mainMenuUI;

    public void HostSessionBtn()
    {
        networkManager.StartHost();
    }
    public void JoinSessionBtn()
    {
        networkManager.StartClient();
    }
    public void StartServerBtn()
    {
        networkManager.StartServer();
    }
    public void QUitGameBtn()
    {
        Application.Quit();
    }
}
