using Core.Network.Listeners;
using Core.Network.Packets.C2S;
using Core.Network.Packets.S2C;

namespace Core.Network;

public class NetworkState
{
	private readonly Dictionary<NetworkSide, IPacketHandler<IPacketListener>> _handlers = new();

	public static NetworkState Login = new NetworkState()
		.Setup(NetworkSide.Client, new PacketHandler<IServerLoginListener>()
			.Register(typeof(HelloC2SPacket), buf => new HelloC2SPacket(buf))
			.Register(typeof(KeyC2SPacket), buf => new KeyC2SPacket(buf)))
		.Setup(NetworkSide.Server, new PacketHandler<IClientLoginListener>()
			.Register(typeof(HelloS2CPacket), buf => new HelloS2CPacket(buf))
			.Register(typeof(SuccessS2CPacket), buf => new SuccessS2CPacket(buf)));

	public static NetworkState Auth = new NetworkState()
		.Setup(NetworkSide.Client, new PacketHandler<IServerAuthListener>()
			.Register(typeof(SignupC2SPacket), buf => new SignupC2SPacket(buf))
			.Register(typeof(LoginC2SPacket), buf => new LoginC2SPacket(buf))
			.Register(typeof(ResetC2SPacket), buf => new ResetC2SPacket(buf))
			.Register(typeof(DeleteC2SPacket), buf => new DeleteC2SPacket(buf))
			.Register(typeof(LogoutC2SPacket), buf => new LogoutC2SPacket(buf))
			.Register(typeof(VerifyC2SPacket), buf => new VerifyC2SPacket(buf))
			.Register(typeof(GuestC2SPacket), buf => new GuestC2SPacket(buf)))
		.Setup(NetworkSide.Server, new PacketHandler<IClientAuthListener>());

	private NetworkState Setup<TPl>(NetworkSide side, IPacketHandler<TPl> handler) where TPl : IPacketListener
	{
		_handlers[side] = (IPacketHandler<IPacketListener>) handler;
		return this;
	}

	public IPacketHandler<IPacketListener> GetPacketHandler(NetworkSide side)
	{
		return _handlers[side];
	}

	public int GetPacketId<TPl>(NetworkSide side, IPacket<TPl> packet) where TPl : IPacketListener
	{
		return GetPacketHandler(side).GetId(packet.GetType());
	}

	public interface IPacketHandler<out TPl> where TPl : IPacketListener
	{
		IPacketHandler<TPl> Register(Type type, Func<PacketBuf, IPacket<TPl>> packetFactory);
		int GetId(Type packet);
		IPacket<IPacketListener>? CreatePacket(int id, PacketBuf buf);
	}

	private class PacketHandler<TPl> : IPacketHandler<TPl> where TPl : IPacketListener
	{
		private Dictionary<Type, int> PacketIds { get; } = new();
		private List<Func<PacketBuf, IPacket<TPl>>> PacketFactories { get; } = new();

		public IPacketHandler<TPl> Register(Type type, Func<PacketBuf, IPacket<TPl>> packetFactory)
		{
			PacketIds[type] = PacketFactories.Count;
			PacketFactories.Add(packetFactory);
			return this;
		}

		public int GetId(Type packet)
		{
			var id = PacketIds[packet];
			return id < 0 ? -1 : id;
		}

		public IPacket<IPacketListener>? CreatePacket(int id, PacketBuf buf)
		{
			return PacketFactories.Count > id
				? (IPacket<IPacketListener>) PacketFactories[id](buf)
				: null;
		}
	}
}