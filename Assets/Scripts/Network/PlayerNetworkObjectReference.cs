using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Collections;
using UnityEngine;

public struct PlayerNetworkObjectReference: INetworkSerializable, IEquatable<PlayerNetworkObjectReference>
{
    public ulong ClientId;
    public FixedString128Bytes ClientName;

    public PlayerNetworkObjectReference(ulong clientId, FixedString128Bytes clientName)
    {
        ClientId = clientId;
        ClientName = clientName;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref ClientName);
    }
    public bool Equals(PlayerNetworkObjectReference other)
    {
        return ClientId == other.ClientId;
    }
}
