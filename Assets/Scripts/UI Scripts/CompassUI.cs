using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CompassUI : NetworkBehaviour
{
    public RawImage compass;
    public NetworkObject localPlayer;

    private void Start()
    {
        if (IsHost || IsClient)
        {
            localPlayer = NetworkManager.Singleton.LocalClient.PlayerObject;
        }

    }

    void Update()
    {
        if (IsHost || IsClient)
        {
            compass.uvRect = new Rect(localPlayer.transform.localEulerAngles.y / 360f, 0, 1, 1);
        }

    }
}
