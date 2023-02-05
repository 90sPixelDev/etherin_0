using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkManager : NetworkBehaviour
{
    /// <summary>
    /// A list of all players connected to the game and their basic info.
    /// </summary>
    private NetworkList<PlayerNetworkObjectReference> _players;
    public NetworkList<PlayerNetworkObjectReference> Players { get => _players; set => _players = value; }

    //[Header("Player References")]
    //[SerializeField] private GameObject mainMenuUI;
    //[SerializeField] private GameObject playerInvMenuUI;
    //[SerializeField] private GameObject debugMenuUI;
    //[SerializeField] private GameObject pointerUI;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Players = new NetworkList<PlayerNetworkObjectReference>();

        //mainMenuUI = GameObject.FindGameObjectWithTag("MainUI");
        //playerInvMenuUI = GameObject.FindGameObjectWithTag("PlayerInvUI");
        //pointerUI = GameObject.Find("PointerUI");
        //debugMenuUI = GameObject.Find("DebugMenuUI");
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;

            foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                HandleClientConnected(client.ClientId);
            }
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;

            foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                HandleClientDisconnected(client.ClientId);
            }
        }

    }

    private void HandleClientConnected(ulong clientId)
    {
        Debug.Log("Running HandleClientConnected!");
        Players.Add(new PlayerNetworkObjectReference(clientId, clientId.ToString()));

        //var playerCharaScript = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.GetComponent<CharacterControllerScript>();

        //playerCharaScript.mainMenuUI = mainMenuUI;
        //playerCharaScript.playerInvMenuUI = playerInvMenuUI;
        //playerCharaScript.pointerUI = playerInvMenuUI;
        //playerCharaScript.debugMenuUI = debugMenuUI;
    }

    private void HandleClientDisconnected(ulong clientId)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].ClientId == clientId)
            {
                Players.RemoveAt(i);
                break;
            }
        }
    }

    public void SendReferences()
    {
        Debug.Log("Running SendReferences Manual!");

        var playerCharaScript = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<CharacterControllerScript>();

        //var playerCharaScript = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.GetComponent<CharacterControllerScript>();

        //playerCharaScript.mainMenuUI = mainMenuUI;
        //playerCharaScript.playerInvMenuUI = playerInvMenuUI;
        //playerCharaScript.pointerUI = playerInvMenuUI;
        //playerCharaScript.debugMenuUI = debugMenuUI;
    }
}
