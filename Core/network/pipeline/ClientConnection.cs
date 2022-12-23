using Core.exceptions;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace Core.network.pipeline; 

public class ClientConnection : SimpleChannelInboundHandler<IPacket<IPacketListener>> {

    public static AttributeKey<NetworkState> ProtocolKey => AttributeKey<NetworkState>.ValueOf("protocol");

    public NetworkSide Side { get; }
    public IChannel? Channel { get; private set; }
    public IPacketListener? Listener { get; private set; }

    public ClientConnection(NetworkSide side) {
        this.Side = side;
    }
    
    public static Task GetConnection(string host, int port) {
        var connection = new ClientConnection(NetworkSide.Client);
        var workerGroup = new MultithreadEventLoopGroup();

        return new Bootstrap()
            .Group(workerGroup)
            .Handler(new ClientConnectionInitializer(connection))
            .Option(ChannelOption.TcpNodelay, true)
            .Option(ChannelOption.SoKeepalive, true)
            .Channel<TcpSocketChannel>()
            .ConnectAsync(host, port);
    }
    
    public void SetState(NetworkState state) {
        Channel?.GetAttribute(ProtocolKey).Set(state);
    }
    
    public NetworkState? GetState() { 
        return Channel?.GetAttribute(ProtocolKey).Get();
    }
    
    public void SetListener(IPacketListener listener) {
        this.Listener = listener;
    }
    
    public IPacketListener? GetListener() {
        return this.Listener;
    }

    public override void ChannelActive(IChannelHandlerContext context) {
        Channel = context.Channel;
        SetState(NetworkState.Login);
    }

    public override async void ChannelInactive(IChannelHandlerContext context) {
        if (Channel is { Open: true }) {
            await Channel.CloseAsync();
        }
    }

    protected override void ChannelRead0(IChannelHandlerContext ctx, IPacket<IPacketListener> msg) {
        if (Listener == null)
            throw new IllegalStateException("Listener was null whilst a message was received");

        msg.Apply(Listener);
    }
    
    public Task Send(IPacket<IPacketListener> packet) {
        return Channel is { Open: true } ?
            Channel.WriteAndFlushAsync(packet) :
            throw new IllegalStateException("Channel was null whilst trying to send a packet");
    }
}

public class ClientConnectionInitializer : ChannelInitializer<ISocketChannel> {
    private ClientConnection Connection { get; }
    
    public ClientConnectionInitializer(ClientConnection connection) {
        Connection = connection;
    }
    
    protected override void InitChannel(ISocketChannel channel) {
        channel.Configuration.SetOption(ChannelOption.TcpNodelay, true);
        channel.Pipeline
            // Here will be the packet decryptor
            .AddLast("decoder", new PacketDecoder(NetworkSide.Server))
            // Here will be the packet encryptor
            .AddLast("encoder", new PacketEncoder(NetworkSide.Client))
            .AddLast("handler", Connection);
    }
}