using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public struct PlayerNetworkObjectReference: INetworkSerializable, IEquatable<PlayerNetworkObjectReference>
{
    public ulong ClientId;
    //public NetworkObjectRef PlayerCam;
    //public NetworkObject PlayerUI;

    public PlayerNetworkObjectReference(ulong clientId)
    {
        ClientId = clientId;
        //PlayerCam = playerCam;
        //PlayerUI = playerUI;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientId);
        //serializer.SerializeValue(ref PlayerCam);
        //serializer.SerializeValue(ref PlayerUI);
    }
    public bool Equals(PlayerNetworkObjectReference other)
    {
        return ClientId == other.ClientId;
    }
}
