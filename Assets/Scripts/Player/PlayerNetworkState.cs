using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkState : NetworkBehaviour
{
    public NetworkVariable<bool> inMenu;
    public NetworkVariable<bool> inMainMenu;

    private void Start()
    {
        inMenu = new NetworkVariable<bool> (false);
        inMainMenu = new NetworkVariable<bool> (false);
    }
}
