using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public partial class KeyC2SPacket : AbstractPacket
{
    public KeyC2SPacket(byte[] key, byte[] iv)
    {
        Key = key;
        Iv = iv;
    }

    public byte[] Key { get; }
    public byte[] Iv { get; }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IServerHandshakeListener handshakeListener)
        {
            handshakeListener.OnKey(this);
        }
    }
}