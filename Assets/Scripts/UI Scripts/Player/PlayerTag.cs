using Unity.Netcode;
using UnityEngine;
using Unity.Collections;

public class PlayerTag : NetworkBehaviour
{
    [SerializeField]
    private NetworkVariable<FixedString32Bytes> playerTagText = new NetworkVariable<FixedString32Bytes>();
}
