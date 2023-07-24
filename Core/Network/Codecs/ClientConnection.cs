using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Core.exceptions;
using DotNetty.Codecs.Compression;
using DotNetty.Common.Utilities;
using DotNetty.Handlers.Tls;
using DotNetty.Transport.Channels;

namespace Core.Network.Codecs;

public class ClientConnection : SimpleChannelInboundHandler<AbstractPacket>, IDisposable
{
    private readonly Dictionary<Guid, TaskCompletionSource<AbstractPacket>> _awaitingResponse = new();

    public ClientConnection(NetworkSide side)
    {
        Side = side;
    }

    public override bool IsSharable => true;
    public static AttributeKey<NetworkState> ProtocolKey => AttributeKey<NetworkState>.ValueOf("protocol");
    public NetworkSide Side { get; }
    public IChannel Channel { get; private set; } = null!;
    public IPacketListener Listener { get; set; } = null!;

    public NetworkState State
    {
        get => Channel.GetAttribute(ProtocolKey).Get();
        set => Channel.GetAttribute(ProtocolKey).Set(value);
    }

    public void Dispose()
    {
        Close();
        GC.SuppressFinalize(this);
    }

    public void Close()
    {
        Channel.CloseSafe();
    }

    public override void ChannelActive(IChannelHandlerContext context)
    {
        Channel = context.Channel;
        State = NetworkState.Handshake;
    }

    public override async void ChannelInactive(IChannelHandlerContext context)
    {
        if (Channel is { IsOpen: true })
        {
            await Channel.CloseAsync();
        }
    }

    protected override void ChannelRead0(IChannelHandlerContext ctx, AbstractPacket msg)
    {
        if (Listener == null)
        {
            throw new IllegalStateException("Listener was null whilst receiving a message");
        }

        if (_awaitingResponse.TryGetValue(msg.Guid, out var value))
        {
            value.SetResult(msg);
            _awaitingResponse.Remove(msg.Guid);
        }

        msg.Apply(Listener);
    }

    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        Console.WriteLine($"Exception: {exception.Message}");
        base.ExceptionCaught(context, exception);
    }

    public Task Send(AbstractPacket packet, Guid? guid = null!)
    {
        packet.Guid = guid ?? Guid.NewGuid();
        if (Channel is { IsOpen: true })
        {
            return Channel.WriteAndFlushAsync(packet);
        }

        throw new IllegalStateException("Channel was null whilst trying to send a packet");
    }

    public async Task<TResult?> SendAndWait<TResult>(AbstractPacket packet, Guid? guid = null!) where TResult : AbstractPacket
    {
        packet.Guid = guid ?? Guid.NewGuid();
        var tcs = new TaskCompletionSource<AbstractPacket>();
        _awaitingResponse[packet.Guid] = tcs;
        if (Channel is { IsOpen: true })
        {
            await Channel.WriteAndFlushAsync(packet);
            var result = await tcs.Task;
            if (result is TResult tResult)
            {
                return tResult;
            }

            return null;
        }

        throw new IllegalStateException("Channel was null whilst trying to send a packet");
    }

    public async Task EnableSecureSocketLayer(X509Certificate2? certificate = null)
    {
        var taskCompletionSource = new TaskCompletionSource();
        // Needs to be temporarily disabled because the
        //  SSL handshake must not have our own packet handlers.
        // Normally, SSL is enabled before any custom packets are sent,
        //  but we need packet handling in order to send the capabilities.
        DisablePacketHandling();

        if (Side == NetworkSide.Server)
        {
            Channel.Pipeline.AddLast("tls", TlsHandler.Server(certificate));
            taskCompletionSource.SetResult();
        }

        if (Side == NetworkSide.Client)
        {
            Channel.Pipeline.AddLast("tls", new TlsHandler(stream =>
                new SslStream(stream, true, (_, _, _, _) =>
                {
                    taskCompletionSource.SetResult();
                    Console.WriteLine("SSL Success");
                    return true;
                }), new ClientTlsSettings("")));
        }

        await taskCompletionSource.Task;
        EnablePacketHandling();
    }

    public void EnableEncryption(Aes key)
    {
        {
            var cryptoTransform = key.CreateEncryptor();
            var packetEncryptor = new PacketEncryptor(cryptoTransform);
            Channel.Pipeline.AddBefore("encoder", "encryptor", packetEncryptor);
        }
        {
            var cryptoTransform = key.CreateDecryptor();
            var packetDecryptor = new PacketDecryptor(cryptoTransform);
            Channel.Pipeline.AddBefore("decoder", "decryptor", packetDecryptor);
        }
    }

    private void DisablePacketHandling()
    {
        foreach (var handler in Channel.Pipeline.ToList())
        {
            Channel.Pipeline.Remove(handler);
        }
    }

    private void EnablePacketHandling()
    {
        EnablePacketHandling(Channel);
    }

    public void EnablePacketHandling(IChannel channel)
    {
        channel.Pipeline
            .AddLast("decompressor", new JZlibDecoder(ZlibWrapper.Gzip))
            // <Here will be the packet decryptor>
            .AddLast("decoder", new PacketDecoder(Side))
            .AddLast("compressor", new JZlibEncoder(ZlibWrapper.Gzip))
            // <Here will be the packet encryptor>
            .AddLast("encoder", new PacketEncoder(!Side))
            .AddLast("handler", this);
    }
}