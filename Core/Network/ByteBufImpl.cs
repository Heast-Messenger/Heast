using System.Buffers;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Common;
using DotNetty.Common.Utilities;

namespace Core.Network;

public class ByteBufImpl : IByteBuffer
{
    private readonly IByteBuffer _parent;

    public ByteBufImpl(IByteBuffer parent)
    {
        _parent = parent;
    }

    public IReferenceCounted Retain()
    {
        return _parent.Retain();
    }

    public IReferenceCounted Retain(int increment)
    {
        return _parent.Retain(increment);
    }

    public IReferenceCounted Touch()
    {
        return _parent.Touch();
    }

    public IReferenceCounted Touch(object hint)
    {
        return _parent.Touch(hint);
    }

    public bool Release()
    {
        return _parent.Release();
    }

    public bool Release(int decrement)
    {
        return _parent.Release(decrement);
    }

    public int ReferenceCount { get; }

    public int CompareTo(IByteBuffer? other)
    {
        return _parent.CompareTo(other);
    }

    public bool Equals(IByteBuffer? other)
    {
        return _parent.Equals(other);
    }

    public IByteBuffer AdjustCapacity(int newCapacity)
    {
        return _parent.AdjustCapacity(newCapacity);
    }

    public IByteBuffer AsReadOnly()
    {
        return _parent.AsReadOnly();
    }

    public IByteBuffer SetWriterIndex(int writerIndex)
    {
        return _parent.SetWriterIndex(writerIndex);
    }

    public IByteBuffer SetReaderIndex(int readerIndex)
    {
        return _parent.SetReaderIndex(readerIndex);
    }

    public IByteBuffer SetIndex(int readerIndex, int writerIndex)
    {
        return _parent.SetIndex(readerIndex, writerIndex);
    }

    public bool IsReadable()
    {
        return _parent.IsReadable();
    }

    public bool IsReadable(int size)
    {
        return _parent.IsReadable(size);
    }

    public bool IsWritable()
    {
        return _parent.IsWritable();
    }

    public bool IsWritable(int size)
    {
        return _parent.IsWritable(size);
    }

    public IByteBuffer Clear()
    {
        return _parent.Clear();
    }

    public IByteBuffer MarkReaderIndex()
    {
        return _parent.MarkReaderIndex();
    }

    public IByteBuffer ResetReaderIndex()
    {
        return _parent.ResetReaderIndex();
    }

    public IByteBuffer MarkWriterIndex()
    {
        return _parent.MarkWriterIndex();
    }

    public IByteBuffer ResetWriterIndex()
    {
        return _parent.ResetWriterIndex();
    }

    public IByteBuffer DiscardReadBytes()
    {
        return _parent.DiscardReadBytes();
    }

    public IByteBuffer DiscardSomeReadBytes()
    {
        return _parent.DiscardSomeReadBytes();
    }

    public IByteBuffer EnsureWritable(int minWritableBytes)
    {
        return _parent.EnsureWritable(minWritableBytes);
    }

    public int EnsureWritable(int minWritableBytes, bool force)
    {
        return _parent.EnsureWritable(minWritableBytes, force);
    }

    public bool GetBoolean(int index)
    {
        return _parent.GetBoolean(index);
    }

    public byte GetByte(int index)
    {
        return _parent.GetByte(index);
    }

    public short GetShort(int index)
    {
        return _parent.GetShort(index);
    }

    public short GetShortLE(int index)
    {
        return _parent.GetShortLE(index);
    }

    public int GetInt(int index)
    {
        return _parent.GetInt(index);
    }

    public int GetIntLE(int index)
    {
        return _parent.GetIntLE(index);
    }

    public long GetLong(int index)
    {
        return _parent.GetLong(index);
    }

    public long GetLongLE(int index)
    {
        return _parent.GetLongLE(index);
    }

    public int GetUnsignedMedium(int index)
    {
        return _parent.GetUnsignedMedium(index);
    }

    public int GetUnsignedMediumLE(int index)
    {
        return _parent.GetUnsignedMediumLE(index);
    }

    public IByteBuffer GetBytes(int index, IByteBuffer destination, int dstIndex, int length)
    {
        return _parent.GetBytes(index, destination, dstIndex, length);
    }

    public IByteBuffer GetBytes(int index, byte[] destination)
    {
        return _parent.GetBytes(index, destination);
    }

    public IByteBuffer GetBytes(int index, byte[] destination, int dstIndex, int length)
    {
        return _parent.GetBytes(index, destination, dstIndex, length);
    }

    public IByteBuffer GetBytes(int index, Stream destination, int length)
    {
        return _parent.GetBytes(index, destination, length);
    }

    public ICharSequence GetCharSequence(int index, int length, Encoding encoding)
    {
        return _parent.GetCharSequence(index, length, encoding);
    }

    public string GetString(int index, int length, Encoding encoding)
    {
        return _parent.GetString(index, length, encoding);
    }

    public IByteBuffer SetBoolean(int index, bool value)
    {
        return _parent.SetBoolean(index, value);
    }

    public IByteBuffer SetByte(int index, int value)
    {
        return _parent.SetByte(index, value);
    }

    public IByteBuffer SetShort(int index, int value)
    {
        return _parent.SetShort(index, value);
    }

    public IByteBuffer SetShortLE(int index, int value)
    {
        return _parent.SetShortLE(index, value);
    }

    public IByteBuffer SetInt(int index, int value)
    {
        return _parent.SetInt(index, value);
    }

    public IByteBuffer SetIntLE(int index, int value)
    {
        return _parent.SetIntLE(index, value);
    }

    public IByteBuffer SetMedium(int index, int value)
    {
        return _parent.SetMedium(index, value);
    }

    public IByteBuffer SetMediumLE(int index, int value)
    {
        return _parent.SetMediumLE(index, value);
    }

    public IByteBuffer SetLong(int index, long value)
    {
        return _parent.SetLong(index, value);
    }

    public IByteBuffer SetLongLE(int index, long value)
    {
        return _parent.SetLongLE(index, value);
    }

    public IByteBuffer SetBytes(int index, IByteBuffer src, int length)
    {
        return _parent.SetBytes(index, src, length);
    }

    public IByteBuffer SetBytes(int index, IByteBuffer src, int srcIndex, int length)
    {
        return _parent.SetBytes(index, src, srcIndex, length);
    }

    public IByteBuffer SetBytes(int index, byte[] src)
    {
        return _parent.SetBytes(index, src);
    }

    public IByteBuffer SetBytes(int index, byte[] src, int srcIndex, int length)
    {
        return _parent.SetBytes(index, src, srcIndex, length);
    }

    public Task<int> SetBytesAsync(int index, Stream src, int length, CancellationToken cancellationToken)
    {
        return _parent.SetBytesAsync(index, src, length, cancellationToken);
    }

    public IByteBuffer SetZero(int index, int length)
    {
        return _parent.SetZero(index, length);
    }

    public int SetCharSequence(int index, ICharSequence sequence, Encoding encoding)
    {
        return _parent.SetCharSequence(index, sequence, encoding);
    }

    public int SetString(int index, string value, Encoding encoding)
    {
        return _parent.SetString(index, value, encoding);
    }

    public bool ReadBoolean()
    {
        return _parent.ReadBoolean();
    }

    public byte ReadByte()
    {
        return _parent.ReadByte();
    }

    public short ReadShort()
    {
        return _parent.ReadShort();
    }

    public short ReadShortLE()
    {
        return _parent.ReadShortLE();
    }

    public int ReadMedium()
    {
        return _parent.ReadMedium();
    }

    public int ReadMediumLE()
    {
        return _parent.ReadMediumLE();
    }

    public int ReadUnsignedMedium()
    {
        return _parent.ReadUnsignedMedium();
    }

    public int ReadUnsignedMediumLE()
    {
        return _parent.ReadUnsignedMediumLE();
    }

    public int ReadInt()
    {
        return _parent.ReadInt();
    }

    public int ReadIntLE()
    {
        return _parent.ReadIntLE();
    }

    public long ReadLong()
    {
        return _parent.ReadLong();
    }

    public long ReadLongLE()
    {
        return _parent.ReadLongLE();
    }

    public IByteBuffer ReadBytes(int length)
    {
        return _parent.ReadBytes(length);
    }

    public IByteBuffer ReadBytes(IByteBuffer destination, int length)
    {
        return _parent.ReadBytes(destination, length);
    }

    public IByteBuffer ReadBytes(IByteBuffer destination, int dstIndex, int length)
    {
        return _parent.ReadBytes(destination, dstIndex, length);
    }

    public IByteBuffer ReadBytes(byte[] destination)
    {
        return _parent.ReadBytes(destination);
    }

    public IByteBuffer ReadBytes(byte[] destination, int dstIndex, int length)
    {
        return _parent.ReadBytes(destination, dstIndex, length);
    }

    public IByteBuffer ReadBytes(Stream destination, int length)
    {
        return _parent.ReadBytes(destination, length);
    }

    public ICharSequence ReadCharSequence(int length, Encoding encoding)
    {
        return _parent.ReadCharSequence(length, encoding);
    }

    public string ReadString(int length, Encoding encoding)
    {
        return _parent.ReadString(length, encoding);
    }

    public IByteBuffer SkipBytes(int length)
    {
        return _parent.SkipBytes(length);
    }

    public IByteBuffer WriteBoolean(bool value)
    {
        return _parent.WriteBoolean(value);
    }

    public IByteBuffer WriteByte(int value)
    {
        return _parent.WriteByte(value);
    }

    public IByteBuffer WriteShort(int value)
    {
        return _parent.WriteShort(value);
    }

    public IByteBuffer WriteShortLE(int value)
    {
        return _parent.WriteShortLE(value);
    }

    public IByteBuffer WriteMedium(int value)
    {
        return _parent.WriteMedium(value);
    }

    public IByteBuffer WriteMediumLE(int value)
    {
        return _parent.WriteMediumLE(value);
    }

    public IByteBuffer WriteInt(int value)
    {
        return _parent.WriteInt(value);
    }

    public IByteBuffer WriteIntLE(int value)
    {
        return _parent.WriteIntLE(value);
    }

    public IByteBuffer WriteLong(long value)
    {
        return _parent.WriteLong(value);
    }

    public IByteBuffer WriteLongLE(long value)
    {
        return _parent.WriteLongLE(value);
    }

    public IByteBuffer WriteBytes(IByteBuffer src, int length)
    {
        return _parent.WriteBytes(src, length);
    }

    public IByteBuffer WriteBytes(IByteBuffer src, int srcIndex, int length)
    {
        return _parent.WriteBytes(src, srcIndex, length);
    }

    public IByteBuffer WriteBytes(byte[] src)
    {
        return _parent.WriteBytes(src);
    }

    public IByteBuffer WriteBytes(byte[] src, int srcIndex, int length)
    {
        return _parent.WriteBytes(src, srcIndex, length);
    }

    public ArraySegment<byte> GetIoBuffer(int index, int length)
    {
        return _parent.GetIoBuffer(index, length);
    }

    public ArraySegment<byte>[] GetIoBuffers(int index, int length)
    {
        return _parent.GetIoBuffers(index, length);
    }

    public ref byte GetPinnableMemoryAddress()
    {
        return ref _parent.GetPinnableMemoryAddress();
    }

    public IntPtr AddressOfPinnedMemory()
    {
        return _parent.AddressOfPinnedMemory();
    }

    public IByteBuffer Duplicate()
    {
        return _parent.Duplicate();
    }

    public IByteBuffer RetainedDuplicate()
    {
        return _parent.RetainedDuplicate();
    }

    public IByteBuffer Unwrap()
    {
        return _parent.Unwrap();
    }

    public IByteBuffer Copy(int index, int length)
    {
        return _parent.Copy(index, length);
    }

    public IByteBuffer Slice()
    {
        return _parent.Slice();
    }

    public IByteBuffer RetainedSlice()
    {
        return _parent.RetainedSlice();
    }

    public IByteBuffer Slice(int index, int length)
    {
        return _parent.Slice(index, length);
    }

    public IByteBuffer RetainedSlice(int index, int length)
    {
        return _parent.RetainedSlice(index, length);
    }

    public IByteBuffer ReadSlice(int length)
    {
        return _parent.ReadSlice(length);
    }

    public IByteBuffer ReadRetainedSlice(int length)
    {
        return _parent.ReadRetainedSlice(length);
    }

    public Task WriteBytesAsync(Stream stream, int length, CancellationToken cancellationToken)
    {
        return _parent.WriteBytesAsync(stream, length, cancellationToken);
    }

    public IByteBuffer WriteZero(int length)
    {
        return _parent.WriteZero(length);
    }

    public int WriteCharSequence(ICharSequence sequence, Encoding encoding)
    {
        return _parent.WriteCharSequence(sequence, encoding);
    }

    public int WriteString(string value, Encoding encoding)
    {
        return _parent.WriteString(value, encoding);
    }

    public int FindLastIndex(int index, int count, Predicate<byte> match)
    {
        return _parent.FindLastIndex(index, count, match);
    }

    public int IndexOf(int fromIndex, int toIndex, byte value)
    {
        return _parent.IndexOf(fromIndex, toIndex, value);
    }

    public int IndexOf(int fromIndex, int toIndex, in ReadOnlySpan<byte> values)
    {
        return _parent.IndexOf(fromIndex, toIndex, values);
    }

    public int IndexOfAny(int fromIndex, int toIndex, byte value0, byte value1)
    {
        return _parent.IndexOfAny(fromIndex, toIndex, value0, value1);
    }

    public int IndexOfAny(int fromIndex, int toIndex, byte value0, byte value1, byte value2)
    {
        return _parent.IndexOfAny(fromIndex, toIndex, value0, value1, value2);
    }

    public int IndexOfAny(int fromIndex, int toIndex, in ReadOnlySpan<byte> values)
    {
        return _parent.IndexOfAny(fromIndex, toIndex, values);
    }

    public int ForEachByte(int index, int length, IByteProcessor processor)
    {
        return _parent.ForEachByte(index, length, processor);
    }

    public int ForEachByteDesc(int index, int length, IByteProcessor processor)
    {
        return _parent.ForEachByteDesc(index, length, processor);
    }

    public void AdvanceReader(int count)
    {
        _parent.AdvanceReader(count);
    }

    public ReadOnlyMemory<byte> GetReadableMemory(int index, int count)
    {
        return _parent.GetReadableMemory(index, count);
    }

    public ReadOnlySpan<byte> GetReadableSpan(int index, int count)
    {
        return _parent.GetReadableSpan(index, count);
    }

    public ReadOnlySequence<byte> GetSequence(int index, int count)
    {
        return _parent.GetSequence(index, count);
    }

    public Memory<byte> GetMemory(int index, int count)
    {
        return _parent.GetMemory(index, count);
    }

    public Span<byte> GetSpan(int index, int count)
    {
        return _parent.GetSpan(index, count);
    }

    public int GetBytes(int index, Span<byte> destination)
    {
        return _parent.GetBytes(index, destination);
    }

    public int GetBytes(int index, Memory<byte> destination)
    {
        return _parent.GetBytes(index, destination);
    }

    public int ReadBytes(Span<byte> destination)
    {
        return _parent.ReadBytes(destination);
    }

    public int ReadBytes(Memory<byte> destination)
    {
        return _parent.ReadBytes(destination);
    }

    public IByteBuffer SetBytes(int index, in ReadOnlySpan<byte> src)
    {
        return _parent.SetBytes(index, src);
    }

    public IByteBuffer SetBytes(int index, in ReadOnlyMemory<byte> src)
    {
        return _parent.SetBytes(index, src);
    }

    public IByteBuffer WriteBytes(in ReadOnlySpan<byte> src)
    {
        return _parent.WriteBytes(src);
    }

    public IByteBuffer WriteBytes(in ReadOnlyMemory<byte> src)
    {
        return _parent.WriteBytes(src);
    }

    public int FindIndex(int index, int count, Predicate<byte> match)
    {
        return _parent.FindIndex(index, count, match);
    }

    public int Capacity => _parent.Capacity;
    public int MaxCapacity => _parent.MaxCapacity;
    public IByteBufferAllocator Allocator => _parent.Allocator;
    public bool IsDirect => _parent.IsDirect;
    public bool IsReadOnly => _parent.IsReadOnly;
    public int ReaderIndex => _parent.ReaderIndex;
    public int WriterIndex => _parent.WriterIndex;
    public int ReadableBytes => _parent.ReadableBytes;
    public int WritableBytes => _parent.WritableBytes;
    public int MaxWritableBytes => _parent.MaxWritableBytes;
    public int MaxFastWritableBytes => _parent.MaxFastWritableBytes;
    public bool IsAccessible => _parent.IsAccessible;
    public bool IsSingleIoBuffer => _parent.IsSingleIoBuffer;
    public int IoBufferCount => _parent.IoBufferCount;
    public bool HasArray => _parent.HasArray;
    public byte[] Array => _parent.Array;
    public bool HasMemoryAddress => _parent.HasMemoryAddress;
    public bool IsContiguous => _parent.IsContiguous;
    public int ArrayOffset => _parent.ArrayOffset;
    public ReadOnlyMemory<byte> UnreadMemory => _parent.UnreadMemory;
    public ReadOnlySpan<byte> UnreadSpan => _parent.UnreadSpan;
    public ReadOnlySequence<byte> UnreadSequence => _parent.UnreadSequence;
    public Memory<byte> FreeMemory => _parent.FreeMemory;
    public Span<byte> FreeSpan => _parent.FreeSpan;

    public void Advance(int count)
    {
        _parent.Advance(count);
    }

    public Memory<byte> GetMemory(int sizeHint = 0)
    {
        return _parent.GetMemory(sizeHint);
    }

    public Span<byte> GetSpan(int sizeHint = 0)
    {
        return _parent.GetSpan(sizeHint);
    }

    public ushort GetUnsignedShort(int index)
    {
        return _parent.GetUnsignedShort(index);
    }

    public ushort GetUnsignedShortLE(int index)
    {
        return _parent.GetUnsignedShortLE(index);
    }

    public uint GetUnsignedInt(int index)
    {
        return _parent.GetUnsignedInt(index);
    }

    public uint GetUnsignedIntLE(int index)
    {
        return _parent.GetUnsignedIntLE(index);
    }

    public int GetMedium(int index)
    {
        return _parent.GetMedium(index);
    }

    public int GetMediumLE(int index)
    {
        return _parent.GetMediumLE(index);
    }

    public char GetChar(int index)
    {
        return _parent.GetChar(index);
    }

    public float GetFloat(int index)
    {
        return _parent.GetFloat(index);
    }

    public float GetFloatLE(int index)
    {
        return _parent.GetFloatLE(index);
    }

    public double GetDouble(int index)
    {
        return _parent.GetDouble(index);
    }

    public double GetDoubleLE(int index)
    {
        return _parent.GetDoubleLE(index);
    }

    public IByteBuffer GetBytes(int index, IByteBuffer destination)
    {
        return _parent.GetBytes(index, destination);
    }

    public IByteBuffer GetBytes(int index, IByteBuffer destination, int length)
    {
        return _parent.GetBytes(index, destination, length);
    }

    public IByteBuffer SetUnsignedShort(int index, ushort value)
    {
        return _parent.SetUnsignedShort(index, value);
    }

    public IByteBuffer SetUnsignedShortLE(int index, ushort value)
    {
        return _parent.SetUnsignedShortLE(index, value);
    }

    public IByteBuffer SetUnsignedInt(int index, uint value)
    {
        return _parent.SetUnsignedInt(index, value);
    }

    public IByteBuffer SetUnsignedIntLE(int index, uint value)
    {
        return _parent.SetUnsignedIntLE(index, value);
    }

    public IByteBuffer SetChar(int index, char value)
    {
        return _parent.SetChar(index, value);
    }

    public IByteBuffer SetDouble(int index, double value)
    {
        return _parent.SetDouble(index, value);
    }

    public IByteBuffer SetFloat(int index, float value)
    {
        return _parent.SetFloat(index, value);
    }

    public IByteBuffer SetDoubleLE(int index, double value)
    {
        return _parent.SetDoubleLE(index, value);
    }

    public IByteBuffer SetFloatLE(int index, float value)
    {
        return _parent.SetFloatLE(index, value);
    }

    public IByteBuffer SetBytes(int index, IByteBuffer src)
    {
        return _parent.SetBytes(index, src);
    }

    public ushort ReadUnsignedShort()
    {
        return _parent.ReadUnsignedShort();
    }

    public ushort ReadUnsignedShortLE()
    {
        return _parent.ReadUnsignedShortLE();
    }

    public uint ReadUnsignedInt()
    {
        return _parent.ReadUnsignedInt();
    }

    public uint ReadUnsignedIntLE()
    {
        return _parent.ReadUnsignedIntLE();
    }

    public char ReadChar()
    {
        return _parent.ReadChar();
    }

    public double ReadDouble()
    {
        return _parent.ReadDouble();
    }

    public double ReadDoubleLE()
    {
        return _parent.ReadDoubleLE();
    }

    public float ReadFloat()
    {
        return _parent.ReadFloat();
    }

    public float ReadFloatLE()
    {
        return _parent.ReadFloatLE();
    }

    public IByteBuffer ReadBytes(IByteBuffer destination)
    {
        return _parent.ReadBytes(destination);
    }

    public IByteBuffer WriteUnsignedShort(ushort value)
    {
        return _parent.WriteUnsignedShort(value);
    }

    public IByteBuffer WriteUnsignedShortLE(ushort value)
    {
        return _parent.WriteUnsignedShortLE(value);
    }

    public IByteBuffer WriteChar(char value)
    {
        return _parent.WriteChar(value);
    }

    public IByteBuffer WriteDouble(double value)
    {
        return _parent.WriteDouble(value);
    }

    public IByteBuffer WriteDoubleLE(double value)
    {
        return _parent.WriteDoubleLE(value);
    }

    public IByteBuffer WriteFloat(float value)
    {
        return _parent.WriteFloat(value);
    }

    public IByteBuffer WriteFloatLE(float value)
    {
        return _parent.WriteFloatLE(value);
    }

    public IByteBuffer WriteBytes(IByteBuffer src)
    {
        return _parent.WriteBytes(src);
    }

    public ArraySegment<byte> GetIoBuffer()
    {
        return _parent.GetIoBuffer();
    }

    public ArraySegment<byte>[] GetIoBuffers()
    {
        return _parent.GetIoBuffers();
    }

    public IByteBuffer Copy()
    {
        return _parent.Copy();
    }

    public Task WriteBytesAsync(Stream stream, int length)
    {
        return _parent.WriteBytesAsync(stream, length);
    }

    public int BytesBefore(byte value)
    {
        return _parent.BytesBefore(value);
    }

    public int BytesBefore(int length, byte value)
    {
        return _parent.BytesBefore(length, value);
    }

    public int BytesBefore(int index, int length, byte value)
    {
        return _parent.BytesBefore(index, length, value);
    }

    public string ToString(Encoding encoding)
    {
        return _parent.ToString(encoding);
    }

    public string ToString(int index, int length, Encoding encoding)
    {
        return _parent.ToString(index, length, encoding);
    }

    public int ForEachByte(IByteProcessor processor)
    {
        return _parent.ForEachByte(processor);
    }

    public int ForEachByteDesc(IByteProcessor processor)
    {
        return _parent.ForEachByteDesc(processor);
    }
}