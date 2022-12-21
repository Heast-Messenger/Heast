using Core.network.listeners;
using Core.network.packets.c2s;
using Core.network.packets.s2c;

namespace Core.network; 

public class NetworkState {

    public static NetworkState Login { get; } = new NetworkState()
        .Setup(NetworkSide.Client, new PacketHandler<IServerLoginListener>()
            .Register(typeof(HelloC2SPacket), buf => new HelloC2SPacket(buf))
            .Register(typeof(KeyC2SPacket), buf => new KeyC2SPacket(buf)))
        .Setup(NetworkSide.Server, new PacketHandler<IClientLoginListener>()
            .Register(typeof(HelloS2CPacket), buf => new HelloS2CPacket(buf))
            .Register(typeof(SuccessS2CPacket), buf => new SuccessS2CPacket(buf)));

    private readonly Dictionary<NetworkSide, IPacketHandler<IPacketListener>> _handlers = new();

    private NetworkState Setup<TPl>(NetworkSide side, IPacketHandler<TPl> handler) where TPl : IPacketListener {
        this._handlers[side] = (IPacketHandler<IPacketListener>) handler;
        return this;
    }

    public IPacketHandler<IPacketListener> GetPacketHandler(NetworkSide side) {
        return _handlers[side];
    }
    
    public int GetPacketId<TPl>(NetworkSide side, IPacket<TPl> packet) where TPl : IPacketListener {
        return GetPacketHandler(side).GetId(packet.GetType());
    }
    
    public interface IPacketHandler<out TPl> where TPl : IPacketListener {
        IPacketHandler<TPl> Register(Type type, Func<PacketBuf, IPacket<TPl>> packetFactory);
        int GetId(Type packet);
    }

    private class PacketHandler<TPl> : IPacketHandler<TPl> where TPl : IPacketListener {
        private Dictionary<Type, int> PacketIds { get; } = new();
        private List<Func<PacketBuf, IPacket<TPl>>> PacketFactories { get; } = new();

        public IPacketHandler<TPl> Register(Type type, Func<PacketBuf, IPacket<TPl>> packetFactory) {
            this.PacketIds[type] = this.PacketFactories.Count;
            this.PacketFactories.Add(packetFactory);
            return this;
        }

        public int GetId(Type packet) {
            var id = this.PacketIds[packet];
            return id < 0 ? -1 : id;
        }

        public IPacket<TPl> CreatePacket(int id, PacketBuf buf) {
            return this.PacketFactories[id](buf);
        }
    }
}