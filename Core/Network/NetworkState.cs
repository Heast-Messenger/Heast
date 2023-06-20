using Core.Network.Listeners;
using Core.Network.Packets.C2S;
using Core.Network.Packets.S2C;

namespace Core.Network;

public class NetworkState
{

	public static NetworkState Login = new NetworkState()
		.Setup(NetworkSide.Client, new PacketHandler<IServerHandshakeListener>()
			.Register<HelloC2SPacket>(buf => new HelloC2SPacket(buf))
			.Register<KeyC2SPacket>(buf => new KeyC2SPacket(buf)))

		.Setup(NetworkSide.Server, new PacketHandler<IClientHandshakeListener>()
			.Register<HelloS2CPacket>(buf => new HelloS2CPacket(buf))
			.Register<SuccessS2CPacket>(buf => new SuccessS2CPacket(buf))
			.Register<ErrorS2CPacket>(buf => new ErrorS2CPacket(buf)));

	public static NetworkState Auth = new NetworkState()
		.Setup(NetworkSide.Client, new PacketHandler<IServerAuthListener>()
			.Register<SignupC2SPacket>(buf => new SignupC2SPacket(buf))
			.Register<LoginC2SPacket>(buf => new LoginC2SPacket(buf))
			.Register<ResetC2SPacket>(buf => new ResetC2SPacket(buf))
			.Register<DeleteC2SPacket>(buf => new DeleteC2SPacket(buf))
			.Register<LogoutC2SPacket>(buf => new LogoutC2SPacket(buf))
			.Register<VerifyC2SPacket>(buf => new VerifyC2SPacket(buf))
			.Register<GuestC2SPacket>(buf => new GuestC2SPacket(buf)))

		.Setup(NetworkSide.Server, new PacketHandler<IClientAuthListener>());

	private readonly Dictionary<NetworkSide, IPacketHandler<IPacketListener>> _handlers = new();

	private NetworkState Setup<TPl>(NetworkSide side, IPacketHandler<TPl> handler) where TPl : IPacketListener
	{
		_handlers[side] = (IPacketHandler<IPacketListener>)handler;
		return this;
	}

	public IPacketHandler<IPacketListener> GetPacketHandler(NetworkSide side)
	{
		return _handlers[side];
	}

	public int GetPacketId(NetworkSide side, IPacket packet)
	{
		return GetPacketHandler(side).GetId(packet.GetType());
	}

	public interface IPacketHandler<out TPl> where TPl : IPacketListener
	{
		IPacketHandler<TPl> Register<TP>(Func<PacketBuf, IPacket> packetFactory) where TP : IPacket;
		int GetId(Type packet);
		IPacket? CreatePacket(int id, PacketBuf buf);
	}

	private class PacketHandler<TPl> : IPacketHandler<TPl> where TPl : IPacketListener
	{
		private Dictionary<Type, int> PacketIds { get; } = new();
		private List<Func<PacketBuf, IPacket>> PacketFactories { get; } = new();

		public IPacketHandler<TPl> Register<TP>(Func<PacketBuf, IPacket> packetFactory) where TP : IPacket
		{
			PacketIds[typeof(TP)] = PacketFactories.Count;
			PacketFactories.Add(packetFactory);
			return this;
		}

		public int GetId(Type packet)
		{
			var id = PacketIds[packet];
			return id < 0 ? -1 : id;
		}

		public IPacket? CreatePacket(int id, PacketBuf buf)
		{
			return PacketFactories.Count > id
				? PacketFactories[id](buf)
				: null;
		}
	}
}