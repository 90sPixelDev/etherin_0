using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkSceneManagment : NetworkBehaviour
{
    public NetworkSceneManager.VerifySceneBeforeLoadingDelegateHandler VerifySceneBeforeLoading;

    public void LoadPlayerIntoWorld()
    {

    }
}
