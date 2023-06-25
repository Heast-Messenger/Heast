using System.Security;
using System.Security.Cryptography;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace Core.Network.Codecs;

public class PacketDecryptor : MessageToMessageDecoder<IByteBuffer>
{
	public PacketDecryptor(Aes keyPair)
	{
		KeyPair = keyPair;
	}

	private Aes KeyPair { get; }

	protected override void Decode(IChannelHandlerContext context, IByteBuffer message, List<object> output)
	{
		var rb = message.ReadableBytes;
		var bytes = new byte[rb];
		message.ReadBytes(bytes);

		try
		{
			var buf = context.Allocator.HeapBuffer();
			var decrypted = KeyPair.DecryptCfb(bytes, KeyPair.IV, PaddingMode.PKCS7);
			buf.WriteBytes(decrypted);
			output.Add(buf);
		}
		catch (CryptographicException e)
		{
			throw new SecurityException("Failed to decrypt packet!");
		}
	}

	public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
	{
		context.DisconnectAsync();
		Console.WriteLine($"Client kicked: {exception.Message}");
	}
}