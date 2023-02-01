using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StartScreenUI : MonoBehaviour
{
    public void HostSessionBtn()
    {
        NetworkManager.Singleton.StartHost();
    }
    public void JoinSessionBtn()
    {
        NetworkManager.Singleton.StartClient();
    }
    public void StartServerBtn()
    {
        NetworkManager.Singleton.StartServer();
    }
    public void QUitGameBtn()
    {
        Application.Quit();
    }
}
