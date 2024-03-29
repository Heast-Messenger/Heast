using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Core.exceptions;
using DotNetty.Common.Utilities;
using DotNetty.Handlers.Tls;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace Core.Network.Codecs;

public class ClientConnection : SimpleChannelInboundHandler<IPacket>
{
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

	public static async Task<ClientConnection> ServerConnect(string host, int port)
	{
		var connection = new ClientConnection(NetworkSide.Client);
		var workerGroup = new MultithreadEventLoopGroup();

		await new Bootstrap()
			.Group(workerGroup)
			.Channel<TcpSocketChannel>()
			.Option(ChannelOption.TcpNodelay, true)
			.Handler(new ClientConnectionInitializer(connection))
			.ConnectAsync(IPAddress.Parse(host), port);

		return connection;
	}

	public override void ChannelActive(IChannelHandlerContext context)
	{
		Channel = context.Channel;
		State = NetworkState.Login;
	}

	public override async void ChannelInactive(IChannelHandlerContext context)
	{
		if (Channel is { IsOpen: true })
		{
			await Channel.CloseAsync();
		}
	}

	protected override void ChannelRead0(IChannelHandlerContext ctx, IPacket msg)
	{
		if (Listener == null)
		{
			throw new IllegalStateException("Listener was null whilst a message was received");
		}

		msg.Apply(Listener);
	}

	public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
	{
		Console.WriteLine($"Exception: {exception.Message}");
		base.ExceptionCaught(context, exception);
	}

	public Task Send(IPacket packet)
	{
		return Channel is { IsOpen: true }
			? Channel.WriteAndFlushAsync(packet)
			: throw new IllegalStateException("Channel was null whilst trying to send a packet");
	}

	public async Task EnableSecureSocketLayer(X509Certificate2? certificate = null)
	{
		var taskCompletionSource = new TaskCompletionSource();
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
		Channel.Pipeline.AddBefore("encoder", "encryptor", new PacketEncryptor(key));
		Channel.Pipeline.AddBefore("decoder", "decryptor", new PacketDecryptor(key));
	}

	public void DisablePacketHandling()
	{
		Channel.Pipeline.Remove("decoder");
		Channel.Pipeline.Remove("encoder");
		Channel.Pipeline.Remove("handler");
	}

	public void EnablePacketHandling()
	{
		EnablePacketHandling(Channel, this);
	}

	public void EnablePacketHandling(IChannel channel, ClientConnection connection)
	{
		channel.Pipeline
			// Here will be the packet decryptor
			.AddLast("decoder", new PacketDecoder(Side))
			// Here will be the packet encryptor
			.AddLast("encoder", new PacketEncoder(!Side))
			.AddLast("handler", connection);
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
		Connection.EnablePacketHandling(channel, Connection);
	}
}