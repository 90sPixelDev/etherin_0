using System.Collections;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;
using Random = UnityEngine.Random;
using UnityEngine;

// Game developer code
public class Lobbies : MonoBehaviour
{
    [SerializeField] private string _lobbyID;

    async void Start()
    {
        // UnityServices.InitializeAsync() will initialize all services that are subscribed to Core.
        await UnityServices.InitializeAsync();
        Debug.Log(UnityServices.State);
        SetupEvents();
        await SignInAnonymouslyAsync();
    }

    #region Lobby
    // Create public lobby
    public async void CreateLobby(string lobbyName, int maxPlayers, bool isSetPrivate, string lobbyCode)
    {
        CreateLobbyOptions options = new CreateLobbyOptions();
        var lobbyData = new Dictionary<string, DataObject>()
        {
            ["Test"] = new DataObject(DataObject.VisibilityOptions.Public, "true", DataObject.IndexOptions.S1),
            ["GameMode"] = new DataObject(DataObject.VisibilityOptions.Public, "Survival", DataObject.IndexOptions.S2),
            ["Skill"] = new DataObject(DataObject.VisibilityOptions.Public, Random.Range(1, 51).ToString(), DataObject.IndexOptions.N1),
            ["Rank"] = new DataObject(DataObject.VisibilityOptions.Public, Random.Range(1, 51).ToString()),
        };

        var lobby = await LobbyService.Instance.CreateLobbyAsync(
                lobbyName: lobbyName,
                maxPlayers: maxPlayers,
                options: new CreateLobbyOptions()
                {
                    Data = lobbyData,
                    IsPrivate = isSetPrivate,
                });

        _lobbyID = lobby.Id;
        Debug.Log($"Created new lobby {lobby.Name} ({_lobbyID})");
    }

    public async void JoinPrivateLobby()
    {

        try
        {
            await LobbyService.Instance.JoinLobbyByIdAsync(_lobbyID);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private void OnDestroy()
    {
        LobbyService.Instance.DeleteLobbyAsync(_lobbyID);
    }

    #endregion

    #region UnityLogging
    async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    // Setup authentication event handlers if desired
    void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

        };

        AuthenticationService.Instance.SignInFailed += (err) =>
        {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.Log("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired.");
        };
    }
    #endregion

}
