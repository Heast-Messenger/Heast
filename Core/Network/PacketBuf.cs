using System.Text;
using DotNetty.Buffers;

namespace Core.Network;

public class PacketBuf : ByteBufImpl
{
	public PacketBuf(IByteBuffer buf) : base(buf)
	{
	}

	public void WriteVarInt(int value)
	{
		while ((value & -128) != 0)
		{
			WriteByte((value & 127) | 128);
			value >>>= 7; //7 because a byte has 8 bits
		}

		WriteByte(value);
	}

	public int ReadVarInt()
	{
		int run = 0, len = 0;
		byte cur;
		do
		{
			cur = ReadByte();
			run |= (cur & 127) << (len++ * 7);
			if (len > 5)
			{
				throw new Exception("VarInt too big");
			}
		} while ((cur & 128) == 128);

		return run;
	}

	public void WriteString(string? str)
	{
		if (str != null)
		{
			WriteVarInt(str.Length);
			WriteBytes(Encoding.UTF8.GetBytes(str));
		}
		else
		{
			WriteVarInt(-1);
		}
	}

	public string ReadString()
	{
		var len = ReadVarInt();
		return len != -1 ? ReadBytes(len).ToString(Encoding.UTF8) : "";
	}

	public void WriteByteArray(byte[] bytes)
	{
		WriteVarInt(bytes.Length);
		WriteBytes(bytes);
	}

	public byte[] ReadByteArray()
	{
		var bytes = new byte[ReadVarInt()];
		ReadBytes(bytes);
		return bytes;
	}

	public void WriteEnum<T>(T value) where T : Enum
	{
		var result = Convert.ToInt32(value);
		WriteVarInt(result);
	}

	public T ReadEnum<T>() where T : Enum
	{
		var flags = ReadVarInt();
		return (T)(object)flags;
	}

	public void WriteTimestamp(DateTime timestamp)
	{
		WriteString(timestamp.ToString("yyyyMMddHHmmss"));
	}

	public DateTime ReadTimestamp()
	{
		return DateTime.ParseExact(ReadString(), "yyyyMMddHHmmss", null);
	}

	public void WriteGuid(Guid guid)
	{
		var result = guid.ToString();
		WriteString(result);
	}

	public Guid ReadGuid()
	{
		var result = ReadString();
		return Guid.Parse(result);
	}
}