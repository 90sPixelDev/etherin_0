using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private GameObject mainMenuUI;

    private void HostSessionBtn()
    {
        networkManager.StartHost();
    }
    private void JoinSessionBtn()
    {
        networkManager.StartClient();
    }
    private void QUitGameBtn()
    {
        Application.Quit();
    }
}
