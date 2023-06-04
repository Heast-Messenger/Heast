using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public class KeyC2SPacket : IPacket<IServerLoginListener>
{

	public KeyC2SPacket(byte[] key, byte[] iv)
	{
		Key = key;
		Iv = iv;
	}

	public KeyC2SPacket(PacketBuf buffer)
	{
		Key = buffer.ReadByteArray();
		Iv = buffer.ReadByteArray();
	}

	public byte[] Key { get; }
	public byte[] Iv { get; }

	public void Write(PacketBuf buf)
	{
		buf.WriteByteArray(Key);
		buf.WriteByteArray(Iv);
	}

	public void Apply(IServerLoginListener listener)
	{
		listener.OnKey(this);
	}
}