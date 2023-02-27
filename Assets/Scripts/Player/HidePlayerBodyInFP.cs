using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HidePlayerBodyInFP : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (IsLocalPlayer)
        {
            transform.gameObject.SetActive(false);
        }
    }
}
