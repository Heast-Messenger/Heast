using System.Security;
using System.Security.Cryptography;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace Core.Network.Codecs;

public class PacketEncryptor : MessageToByteEncoder<IByteBuffer>
{
	public PacketEncryptor(Aes keyPair)
	{
		KeyPair = keyPair;
	}

	private Aes KeyPair { get; }

	protected override void Encode(IChannelHandlerContext context, IByteBuffer message, IByteBuffer output)
	{
		var rb = message.ReadableBytes;
		var bytes = new byte[rb];
		message.ReadBytes(bytes);

		try
		{
			var encrypted = KeyPair.EncryptCfb(bytes, KeyPair.IV, PaddingMode.PKCS7);
			output.WriteBytes(encrypted);
		}
		catch (CryptographicException)
		{
			throw new SecurityException("Failed to encrypt packet!");
		}
	}

	public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
	{
		context.DisconnectAsync();
		Console.WriteLine($"Client kicked: {exception.Message}");
	}
}