using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DontDestroyGO : NetworkBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
