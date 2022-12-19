using System;
using System.Collections.Generic;
using Core.network.listeners;
using Core.network.packets.c2s;

namespace Core.network; 

public class NetworkState {

    public static NetworkState Login = new NetworkState(new PacketHandlerInitializer()
        .Setup(NetworkSide.Client, new PacketHandler<IServerLoginListener>()
            .Register(typeof(HelloC2SPacket), buf => new HelloC2SPacket(buf))
            .Register(typeof(KeyC2SPacket), buf => new KeyC2SPacket(buf)))
        // .Setup(NetworkSide.Server, new PacketHandler<IClientLoginListener>()
        //     .Register()
        //     .Register())
    );

    private readonly IDictionary<NetworkSide, PacketHandler<IPacketListener>> _handlers;

    public NetworkState(PacketHandlerInitializer initializer) {
        _handlers = initializer.Handlers;
    }
    
    public PacketHandler<TPl> GetPacketHandler<TPl>(NetworkSide side) where TPl : IPacketListener {
        return (_handlers[side] as PacketHandler<TPl>)!;
    }
    
    public int GetPacketId<TPl>(NetworkSide side, IPacket<TPl> packet) where TPl : IPacketListener {
        return GetPacketHandler<TPl>(side).GetId(packet.GetType());
    }
    
    public class PacketHandlerInitializer {
        public readonly Dictionary<NetworkSide, PacketHandler<IPacketListener>> Handlers = new();
        
        public PacketHandlerInitializer Setup<TPl>(NetworkSide side, PacketHandler<TPl> handler) where TPl : IPacketListener {
            Handlers[side] = handler; // Hüfe (ich frag Puljic, vllt kennt der sich ja aus)
            return this;
        }
    }

    public class PacketHandler<TPl> where TPl : IPacketListener {
        private readonly Dictionary<Type, int> _packetIds = new();
        private readonly List<Func<PacketBuf, IPacket<TPl>>> _packetFactories = new();

        public PacketHandler<TPl> Register(Type type, Func<PacketBuf, IPacket<TPl>> packetFactory) {
            _packetIds[type] = _packetFactories.Count;
            _packetFactories.Add(packetFactory);
            return this;
        }

        public int GetId(Type packet) {
            var id = _packetIds[packet];
            return id < 0 ? -1 : id;
        }

        public IPacket<TPl> CreatePacket(int id, PacketBuf buf) {
            return _packetFactories[id](buf);
        }
    }
}