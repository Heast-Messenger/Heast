using System.Reflection;
using Core.Network.Listeners;
using Core.Network.Packets.C2S;
using Core.Network.Packets.S2C;

namespace Core.Network;

public class NetworkState
{

	public static NetworkState Login = new NetworkState()
		.Setup(NetworkSide.Client, new PacketHandler<IServerLoginListener>()
			.Register<HelloC2SPacket>(buf => new HelloC2SPacket(buf), (listener, packet) => listener.OnHello(packet))
			.Register<KeyC2SPacket>(buf => new KeyC2SPacket(buf), (listener, packet) => listener.OnKey(packet)))
		.Setup(NetworkSide.Server, new PacketHandler<IClientLoginListener>()
			.Register<HelloS2CPacket>(buf => new HelloS2CPacket(buf), (listener, packet) => listener.OnHello(packet))
			.Register<SuccessS2CPacket>(buf => new SuccessS2CPacket(buf), (listener, packet) => listener.OnSuccess()));

	public static NetworkState Auth = new NetworkState()
//		.Setup(NetworkSide.Client, new PacketHandler<IServerAuthListener>()
//			.Register(typeof(SignupC2SPacket), buf => new SignupC2SPacket(buf))
//			.Register(typeof(LoginC2SPacket), buf => new LoginC2SPacket(buf))
//			.Register(typeof(ResetC2SPacket), buf => new ResetC2SPacket(buf))
//			.Register(typeof(DeleteC2SPacket), buf => new DeleteC2SPacket(buf))
//			.Register(typeof(LogoutC2SPacket), buf => new LogoutC2SPacket(buf))
//			.Register(typeof(VerifyC2SPacket), buf => new VerifyC2SPacket(buf))
//			.Register(typeof(GuestC2SPacket), buf => new GuestC2SPacket(buf)))
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
		IPacketHandler<TPl> Register<TP>(Func<PacketBuf, IPacket> packetFactory, Action<TPl, TP> packetApply) where TP : IPacket;
		int GetId(Type packet);
		IPacket? CreatePacket(int id, PacketBuf buf);
		void ApplyPacket(IPacket packet, IPacketListener listener);
	}

	private class PacketHandler<TPl> : IPacketHandler<TPl> where TPl : IPacketListener
	{
		private Dictionary<Type, int> PacketIds { get; } = new();
		private List<Func<PacketBuf, IPacket>> PacketFactories { get; } = new();
		private List<MethodInfo> PacketApplies { get; } = new();

		public IPacketHandler<TPl> Register<TP>(Func<PacketBuf, IPacket> packetFactory, Action<TPl, TP> packetApply) where TP : IPacket
		{
			PacketIds[typeof(TP)] = PacketFactories.Count;
			PacketFactories.Add(packetFactory);
			PacketApplies.Add(packetApply.Method);
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

		public void ApplyPacket(IPacket packet, IPacketListener listener)
		{
			var id = GetId(packet.GetType());
			if (PacketApplies.Count > id)
			{
				PacketApplies[id].Invoke(listener, parameters: new []{ packet });
			}
		}
	}
}