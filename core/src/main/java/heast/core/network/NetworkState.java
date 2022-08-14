package heast.core.network;

import heast.core.network.c2s.*;
import heast.core.network.c2s.listener.ServerAuthListener;
import heast.core.network.s2c.*;
import heast.core.network.s2c.listener.ClientAuthListener;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Function;

/**
 * A network state defines a state a client can be in the network.
 * States have a unique id and a list of packets that can be sent to the client / server and vice versa.
 */
public enum NetworkState {
    AUTHENTICATION(new PacketHandlerInitializer()
        .setup(NetworkSide.CLIENT, new PacketHandler<ServerAuthListener>()
            .register(LoginC2SPacket.class, LoginC2SPacket::new)
            .register(SignupC2SPacket.class, SignupC2SPacket::new)
            .register(ResetC2SPacket.class, ResetC2SPacket::new)
            .register(LogoutC2SPacket.class, LogoutC2SPacket::new)
            .register(VerificationC2SPacket.class, VerificationC2SPacket::new)
            .register(ServerKeyC2SPacket.class, ServerKeyC2SPacket::new)
        )
        .setup(NetworkSide.SERVER, new PacketHandler<ClientAuthListener>()
            .register(LoginResponseS2CPacket.class, LoginResponseS2CPacket::new)
            .register(SignupResponseS2CPacket.class, SignupResponseS2CPacket::new)
            .register(ResetResponseS2CPacket.class, ResetResponseS2CPacket::new)
            .register(ServerKeyResponseS2CPacket.class, ServerKeyResponseS2CPacket::new)
        )
    );

    private final Map<NetworkSide, ? extends PacketHandler<? extends PacketListener>> handlers;

    /**
     * Creates a new State with a list of handlers.
     * Handlers are mapped to their respective side in the network, either CLIENT or SERVER.
     * @param initializer The initializer that creates the handlers.
     */
    NetworkState(PacketHandlerInitializer initializer) {
        this.handlers = initializer.handlers;
    }

    /**
     * Gets a handler for a given side.
     * @param side The side to get the handler for.
     * @return The handler for the given side.
     */
    public PacketHandler<? extends PacketListener> getPacketHandler(NetworkSide side) {
        return handlers.get(side);
    }

    public Integer getPacketId(NetworkSide side, Packet<?> packet) {
        return handlers.get(side).getId(packet.getClass());
    }

    /**
     * Utility class for initializing packet handlers.
     * It simply contains a separate handler for each side in the network in form of a map.
     * These can be then registered using the {@link #setup(NetworkSide, PacketHandler)} method.
     */
    public static class PacketHandlerInitializer {
        final Map<NetworkSide, PacketHandler<?>> handlers = new HashMap<>();

        /**
         * Initializes a packet handler for a given side.
         * @param side The side to set up the handler for.
         * @param handler The handler to set up.
         * @return This initializer for chaining.
         * @param <PL> A packet listener extending the {@link PacketListener} interface.
         */
        public <PL extends PacketListener> PacketHandlerInitializer setup(NetworkSide side, PacketHandler<PL> handler) {
            this.handlers.put(side, handler);
            return this;
        }
    }

    /**
     * A packet handler is a function that creates a packet from a packet id and a packet buffer.
     * Packet Handlers can contain multiple packets in the same "area" of the network, and these are stored in the packetIds map.
     * For example, the Login, Signup and Account Reset packets are all handled by the same handler, the {@link heast.authserver.network.ServerAuthHandler}.
     *
     * The packetFactories list is used to construct the packets from a {@link PacketBuf} instance.
     * The constructor to use to create the packet is determined by the packetFactory parameter.
     * @see #register(Class, Function)
     *
     * @param <PL> Some packet listener type extending the {@link PacketListener} interface.
     */
    public static class PacketHandler<PL extends PacketListener> {
        private final Map<Class<? extends Packet<PL>>, Integer> packetIds = new HashMap<>();
        private final List<Function<PacketBuf, ? extends Packet<PL>>> packetFactories = new ArrayList<>();

        /**
         * Registers a packet within the handler.
         * @param type The type of packet to register.
         * @param packetFactory The function (constructor) to use to create the packet.
         * @return This handler for chaining.
         * @param <P> Some packet type extending the base Packet.
         */
        public <P extends Packet<PL>> PacketHandler<PL> register(Class<P> type, Function<PacketBuf, P> packetFactory) {
            this.packetIds.put(
                type, packetFactories.size()
            );
            this.packetFactories.add(packetFactory);
            return this;
        }

        /**
         * Gets the packet id for a given packet class.
         * @param packet The packet class to get the according id for.
         * @return The packet id.
         */
        public int getId(Class<?> packet) {
            int id = this.packetIds.get(packet);
            return id < 0 ? -1 : id;
        }

        /**
         * Creates a packet from a packet id and a packet buffer.
         * @param id The packet id.
         * @param buf The packet buffer.
         * @return The packet.
         */
        public Packet<?> createPacket(int id, PacketBuf buf) {
            Function<PacketBuf, ? extends Packet<PL>> fun = this.packetFactories.get(id);
            return fun != null ? fun.apply(buf) : null;
        }
    }
}
