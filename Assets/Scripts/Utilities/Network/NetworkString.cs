using Unity.Collections;
using Unity.Netcode;

public struct NetworkString : INetworkSerializable
{
    private FixedString32Bytes text;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref text);
    }

    public override string ToString()
    {
        return text.ToString();
    }

    public static implicit operator string(NetworkString s) { return s.ToString(); }
    public static implicit operator NetworkString(string s) { return new NetworkString() { text = new FixedString32Bytes(s) }; }
}
