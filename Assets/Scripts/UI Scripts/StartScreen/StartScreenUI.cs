using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartScreenUI : NetworkBehaviour
{
    private ProjectNetworkSceneManager projNetSceneManager;
    [SerializeField]
    private TMP_InputField ipInputField;
    [SerializeField]
    private GameObject optionsUI;
    private EventSystem eventSystem;
    [SerializeField]
    private GameObject mouseXSenSlider;
    [SerializeField]
    private GameObject hostGameBtn;

    private void Awake()
    {
        projNetSceneManager = GameObject.Find("NetworkSceneManager").GetComponent<ProjectNetworkSceneManager>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    public void HostSessionBtn()
    {
        SetNetworkConfig("server");
        NetworkManager.Singleton.StartHost();
    }
    public void JoinSessionBtn()
    {
        SetNetworkConfig("join");
        NetworkManager.Singleton.StartClient();
    }
    public void StartServerBtn()
    {
        SetNetworkConfig("server");
        NetworkManager.Singleton.StartServer();
    }

    public void GoToOptionsScreen()
    {
        optionsUI.SetActive(true);
        eventSystem.SetSelectedGameObject(mouseXSenSlider);
    }

    public void ReturnToStartScreen()
    {
        optionsUI.SetActive(false);
        eventSystem.SetSelectedGameObject(hostGameBtn);
    }
    public void QUitGameBtn()
    {
        Application.Quit();
    }

    public void OnIPFieldChange()
    {
        projNetSceneManager.UpdateListenIP(ipInputField.text);
    }

    private void SetNetworkConfig(string startType)
    {
        switch (startType)
        {
            case "server":
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(GetUserPublicIP(), (ushort)27007, "0.0.0.0");
                break;
            case "join":
                if (ipInputField.text == "0.0.0.0" || ipInputField.text == "localhost")
                {
                    ipInputField.text = "127.0.0.1";
                }
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipInputField.text, (ushort)27007);
                break;
        }
    }

    private string GetUserPublicIP()
    {
        string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
        var externalIp = IPAddress.Parse(externalIpString);

        return externalIp.ToString();
    }
}
