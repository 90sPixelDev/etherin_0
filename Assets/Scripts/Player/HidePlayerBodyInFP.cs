using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HidePlayerBodyInFP : NetworkBehaviour
{
    [SerializeField] private GameObject playerBody;

    // Start is called before the first frame update
    void Start()
    {
        if (IsLocalPlayer)
        {
            playerBody.SetActive(false);
        }
    }
}
