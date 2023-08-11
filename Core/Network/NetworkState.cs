using Core.Network.Listeners;
using Core.Network.Packets.C2S;
using Core.Network.Packets.S2C;

namespace Core.Network;

public class NetworkState
{
    public static readonly NetworkState Handshake = new NetworkState()
        .Setup(NetworkSide.Server,
            new PacketHandler<IServerHandshakeListener>()
                .Register<HelloC2SPacket>(buf => new HelloC2SPacket(buf))
                .Register<ConnectC2SPacket>(buf => new ConnectC2SPacket(buf))
                .Register<KeyC2SPacket>(buf => new KeyC2SPacket(buf))
                .Register<PingC2SPacket>(buf => new PingC2SPacket(buf)))
        .Setup(NetworkSide.Client,
            new PacketHandler<IClientHandshakeListener>()
                .Register<HelloS2CPacket>(buf => new HelloS2CPacket(buf))
                .Register<ConnectS2CPacket>(buf => new ConnectS2CPacket(buf))
                .Register<KeyS2CPacket>(buf => new KeyS2CPacket(buf))
                .Register<PingS2CPacket>(buf => new PingS2CPacket(buf)));

    public static readonly NetworkState Auth = new NetworkState()
        .Setup(NetworkSide.Server,
            new PacketHandler<IServerAuthListener>()
                .Register<SignupC2SPacket>(buf => new SignupC2SPacket(buf))
                .Register<LoginC2SPacket>(buf => new LoginC2SPacket(buf))
                .Register<ResetC2SPacket>(buf => new ResetC2SPacket(buf))
                .Register<DeleteC2SPacket>(buf => new DeleteC2SPacket(buf))
                .Register<LogoutC2SPacket>(buf => new LogoutC2SPacket(buf))
                .Register<VerifyEmailC2SPacket>(buf => new VerifyEmailC2SPacket(buf))
                .Register<GuestC2SPacket>(buf => new GuestC2SPacket(buf))
                .Register<AccountRequestC2SPacket>(buf => new AccountRequestC2SPacket(buf)))
        .Setup(NetworkSide.Client,
            new PacketHandler<IClientAuthListener>()
                .Register<SignupS2CPacket>(buf => new SignupS2CPacket(buf))
                .Register<VerifyEmailS2CPacket>(buf => new VerifyEmailS2CPacket(buf))
                .Register<AccountRequestS2CPacket>(buf => new AccountRequestS2CPacket(buf)));

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

    public int GetPacketId(NetworkSide side, AbstractPacket packet)
    {
        return GetPacketHandler(side).GetId(packet.GetType());
    }

    public interface IPacketHandler<out TPl> where TPl : IPacketListener
    {
        IPacketHandler<TPl> Register<TP>(Func<PacketBuf, AbstractPacket> packetFactory) where TP : AbstractPacket;
        int GetId(Type packet);
        AbstractPacket? CreatePacket(int id, PacketBuf buf);
    }

    private class PacketHandler<TPl> : IPacketHandler<TPl> where TPl : IPacketListener
    {
        private Dictionary<Type, int> PacketIds { get; } = new();
        private List<Func<PacketBuf, AbstractPacket>> PacketFactories { get; } = new();

        public IPacketHandler<TPl> Register<TP>(Func<PacketBuf, AbstractPacket> packetFactory) where TP : AbstractPacket
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

        public AbstractPacket? CreatePacket(int id, PacketBuf buf)
        {
            return PacketFactories.Count > id
                ? PacketFactories[id](buf)
                : null;
        }
    }
}