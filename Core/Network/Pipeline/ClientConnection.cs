using System.Security.Cryptography;
using Core.exceptions;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace Core.Network.Pipeline;

public class ClientConnection : SimpleChannelInboundHandler<IPacket<IPacketListener>>
{
    public ClientConnection(NetworkSide side)
    {
        Side = side;
    }

    public static AttributeKey<NetworkState> ProtocolKey => AttributeKey<NetworkState>.ValueOf("protocol");

    public NetworkSide Side { get; }

    public IChannel Channel { get; set; } = null!;

    public IPacketListener Listener { get; set; } = null!;

    public NetworkState State
    {
        get => Channel.GetAttribute(ProtocolKey).Get();
        set => Channel.GetAttribute(ProtocolKey).Set(value);
    }

    public static Task GetConnection(string host, int port)
    {
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

    public override void ChannelActive(IChannelHandlerContext context)
    {
        Channel = context.Channel;
        State = NetworkState.Login;
    }

    public override async void ChannelInactive(IChannelHandlerContext context)
    {
        if (Channel is { Open: true }) await Channel.CloseAsync();
    }

    protected override void ChannelRead0(IChannelHandlerContext ctx, IPacket<IPacketListener> msg)
    {
        if (Listener == null)
            throw new IllegalStateException("Listener was null whilst a message was received");

        msg.Apply(Listener);
    }

    public Task Send<T>(IPacket<T> packet) where T : IPacketListener
    {
        return Channel is { Open: true }
            ? Channel.WriteAndFlushAsync(packet)
            : throw new IllegalStateException("Channel was null whilst trying to send a packet");
    }

    public void EnableEncryption(Aes key)
    {
        Channel.Pipeline.AddBefore("encoder", "encryptor", new PacketEncryptor(key));
        Channel.Pipeline.AddBefore("decoder", "decryptor", new PacketDecryptor(key));
    }
}

public class ClientConnectionInitializer : ChannelInitializer<ISocketChannel>
{
    public ClientConnectionInitializer(ClientConnection connection)
    {
        Connection = connection;
    }

    private ClientConnection Connection { get; }

    protected override void InitChannel(ISocketChannel channel)
    {
        channel.Configuration.SetOption(ChannelOption.TcpNodelay, true);
        channel.Configuration.SetOption(ChannelOption.SoKeepalive, true);
        channel.Pipeline
            // Here will be the packet decryptor
            .AddLast("decoder", new PacketDecoder(NetworkSide.Server))
            // Here will be the packet encryptor
            .AddLast("encoder", new PacketEncoder(NetworkSide.Client))
            .AddLast("handler", Connection);
    }
}