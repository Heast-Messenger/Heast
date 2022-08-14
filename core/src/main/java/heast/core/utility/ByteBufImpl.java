package heast.core.utility;

import io.netty.buffer.ByteBuf;
import io.netty.buffer.ByteBufAllocator;
import io.netty.buffer.ByteBufProcessor;
import io.netty.buffer.Unpooled;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.nio.channels.GatheringByteChannel;
import java.nio.channels.ScatteringByteChannel;
import java.nio.charset.Charset;
import java.nio.charset.UnsupportedCharsetException;

public class ByteBufImpl extends ByteBuf {
    private final ByteBuf parent;

    public ByteBufImpl(ByteBuf parent) {
        this.parent = parent;
    }

    /**
     * Returns the number of bytes (octets) this buffer can contain.
     */
    @Override
    public int capacity() {
        return parent.capacity();
    }

    /**
     * Adjusts the capacity of this buffer.  If the {@code newCapacity} is less than the current
     * capacity, the content of this buffer is truncated.  If the {@code newCapacity} is greater
     * than the current capacity, the buffer is appended with unspecified data whose length is
     * {@code (newCapacity - currentCapacity)}.
     *
     */
    @Override
    public ByteBuf capacity(int newCapacity) {
        return parent.capacity(newCapacity);
    }

    /**
     * Returns the maximum allowed capacity of this buffer.  If a user attempts to increase the
     * capacity of this buffer beyond the maximum capacity using {@link #capacity(int)} or
     * {@link #ensureWritable(int)}, those methods will raise an
     * {@link IllegalArgumentException}.
     */
    @Override
    public int maxCapacity() {
        return parent.maxCapacity();
    }

    /**
     * Returns the {@link ByteBufAllocator} which created this buffer.
     */
    @Override
    public ByteBufAllocator alloc() {
        return parent.alloc();
    }

    /**
     * Returns the <a href="http://en.wikipedia.org/wiki/Endianness">endianness</a>
     * of this buffer.
     */
    @Override
    public ByteOrder order() {
        return parent.order();
    }

    /**
     * Returns a buffer with the specified {@code endianness} which shares the whole region,
     * indexes, and marks of this buffer.  Modifying the content, the indexes, or the marks of the
     * returned buffer or this buffer affects each other's content, indexes, and marks.  If the
     * specified {@code endianness} is identical to this buffer's byte order, this method can
     * return {@code this}.  This method does not modify {@code readerIndex} or {@code writerIndex}
     * of this buffer.
     *
     */
    @Override
    public ByteBuf order(ByteOrder endianness) {
        return parent.order(endianness);
    }

    /**
     * Return the underlying buffer instance if this buffer is a wrapper of another buffer.
     *
     * @return {@code null} if this buffer is not a wrapper
     */
    @Override
    public ByteBuf unwrap() {
        return parent.unwrap();
    }

    /**
     * Returns {@code true} if and only if this buffer is backed by an
     * NIO direct buffer.
     */
    @Override
    public boolean isDirect() {
        return parent.isDirect();
    }

    /**
     * Returns the {@code readerIndex} of this buffer.
     */
    @Override
    public int readerIndex() {
        return parent.readerIndex();
    }

    /**
     * Sets the {@code readerIndex} of this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code readerIndex} is
     * less than {@code 0} or
     * greater than {@code this.writerIndex}
     */
    @Override
    public ByteBuf readerIndex(int readerIndex) {
        return parent.readerIndex(readerIndex);
    }

    /**
     * Returns the {@code writerIndex} of this buffer.
     */
    @Override
    public int writerIndex() {
        return parent.writerIndex();
    }

    /**
     * Sets the {@code writerIndex} of this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code writerIndex} is
     * less than {@code this.readerIndex} or
     * greater than {@code this.capacity}
     */
    @Override
    public ByteBuf writerIndex(int writerIndex) {
        return parent.writerIndex(writerIndex);
    }

    /**
     * Sets the {@code readerIndex} and {@code writerIndex} of this buffer
     * in one shot.  This method is useful when you have to worry about the
     * invocation order of {@link #readerIndex(int)} and {@link #writerIndex(int)}
     * methods.  For example, the following code will fail:
     *
     * <pre>
     * // Create a buffer whose readerIndex, writerIndex and capacity are
     * // 0, 0 and 8 respectively.
     * {@link ByteBuf} buf = {@link Unpooled}.buffer(8);
     *
     * // IndexOutOfBoundsException is thrown because the specified
     * // readerIndex (2) cannot be greater than the current writerIndex (0).
     * buf.readerIndex(2);
     * buf.writerIndex(4);
     * </pre>
     * <p>
     * The following code will also fail:
     *
     * <pre>
     * // Create a buffer whose readerIndex, writerIndex and capacity are
     * // 0, 8 and 8 respectively.
     * {@link ByteBuf} buf = {@link Unpooled}.wrappedBuffer(new byte[8]);
     *
     * // readerIndex becomes 8.
     * buf.readLong();
     *
     * // IndexOutOfBoundsException is thrown because the specified
     * // writerIndex (4) cannot be less than the current readerIndex (8).
     * buf.writerIndex(4);
     * buf.readerIndex(2);
     * </pre>
     * <p>
     * By contrast, this method guarantees that it never
     * throws an {@link IndexOutOfBoundsException} as long as the specified
     * indexes meet basic constraints, regardless what the current index
     * values of the buffer are:
     *
     * <pre>
     * // No matter what the current state of the buffer is, the following
     * // call always succeeds as long as the capacity of the buffer is not
     * // less than 4.
     * buf.setIndex(2, 4);
     * </pre>
     *
     * @throws IndexOutOfBoundsException if the specified {@code readerIndex} is less than 0,
     * if the specified {@code writerIndex} is less than the specified
     * {@code readerIndex} or if the specified {@code writerIndex} is
     * greater than {@code this.capacity}
     */
    @Override
    public ByteBuf setIndex(int readerIndex, int writerIndex) {
        return parent.setIndex(readerIndex, writerIndex);
    }

    /**
     * Returns the number of readable bytes which is equal to
     * {@code (this.writerIndex - this.readerIndex)}.
     */
    @Override
    public int readableBytes() {
        return parent.readableBytes();
    }

    /**
     * Returns the number of writable bytes which is equal to
     * {@code (this.capacity - this.writerIndex)}.
     */
    @Override
    public int writableBytes() {
        return parent.writableBytes();
    }

    /**
     * Returns the maximum possible number of writable bytes, which is equal to
     * {@code (this.maxCapacity - this.writerIndex)}.
     */
    @Override
    public int maxWritableBytes() {
        return parent.maxWritableBytes();
    }

    /**
     * Returns {@code true}
     * if and only if {@code (this.writerIndex - this.readerIndex)} is greater
     * than {@code 0}.
     */
    @Override
    public boolean isReadable() {
        return parent.isReadable();
    }

    /**
     * Returns {@code true} if and only if this buffer contains equal to or more than the specified number of elements.
     *
     */
    @Override
    public boolean isReadable(int size) {
        return parent.isReadable(size);
    }

    /**
     * Returns {@code true}
     * if and only if {@code (this.capacity - this.writerIndex)} is greater
     * than {@code 0}.
     */
    @Override
    public boolean isWritable() {
        return parent.isWritable();
    }

    /**
     * Returns {@code true} if and only if this buffer has enough room to allow writing the specified number of
     * elements.
     *
     */
    @Override
    public boolean isWritable(int size) {
        return parent.isWritable(size);
    }

    /**
     * Sets the {@code readerIndex} and {@code writerIndex} of this buffer to
     * {@code 0}.
     * This method is identical to {@link #setIndex(int, int) setIndex(0, 0)}.
     * <p>
     * Please take in account that the behavior of this method is different
     * from that of NIO buffer, which sets the {@code limit} to
     * the {@code capacity} of the buffer.
     */
    @Override
    public ByteBuf clear() {
        return parent.clear();
    }

    /**
     * Marks the current {@code readerIndex} in this buffer.  You can
     * reposition the current {@code readerIndex} to the marked
     * {@code readerIndex} by calling {@link #resetReaderIndex()}.
     * The initial value of the marked {@code readerIndex} is {@code 0}.
     */
    @Override
    public ByteBuf markReaderIndex() {
        return parent.markReaderIndex();
    }

    /**
     * Repositions the current {@code readerIndex} to the marked
     * {@code readerIndex} in this buffer.
     *
     * @throws IndexOutOfBoundsException if the current {@code writerIndex} is less than the marked
     * {@code readerIndex}
     */
    @Override
    public ByteBuf resetReaderIndex() {
        return parent.resetReaderIndex();
    }

    /**
     * Marks the current {@code writerIndex} in this buffer.  You can
     * reposition the current {@code writerIndex} to the marked
     * {@code writerIndex} by calling {@link #resetWriterIndex()}.
     * The initial value of the marked {@code writerIndex} is {@code 0}.
     */
    @Override
    public ByteBuf markWriterIndex() {
        return parent.markWriterIndex();
    }

    /**
     * Repositions the current {@code writerIndex} to the marked
     * {@code writerIndex} in this buffer.
     *
     * @throws IndexOutOfBoundsException if the current {@code readerIndex} is greater than the marked
     * {@code writerIndex}
     */
    @Override
    public ByteBuf resetWriterIndex() {
        return parent.resetWriterIndex();
    }

    /**
     * Discards the bytes between the 0th index and {@code readerIndex}.
     * It moves the bytes between {@code readerIndex} and {@code writerIndex}
     * to the 0th index, and sets {@code readerIndex} and {@code writerIndex}
     * to {@code 0} and {@code oldWriterIndex - oldReaderIndex} respectively.
     * <p>
     * Please refer to the class documentation for more detailed explanation.
     */
    @Override
    public ByteBuf discardReadBytes() {
        return parent.discardReadBytes();
    }

    /**
     * Similar to {@link ByteBuf#discardReadBytes()} except that this method might discard
     * some, all, or none of read bytes depending on its internal implementation to reduce
     * overall memory bandwidth consumption at the cost of potentially additional memory
     * consumption.
     */
    @Override
    public ByteBuf discardSomeReadBytes() {
        return parent.discardSomeReadBytes();
    }

    /**
     * Makes sure the number of {@linkplain #writableBytes() the writable bytes}
     * is equal to or greater than the specified value.  If there is enough
     * writable bytes in this buffer, this method returns with no side effect.
     * Otherwise, it raises an {@link IllegalArgumentException}.
     *
     * @param minWritableBytes the expected minimum number of writable bytes
     * @throws IndexOutOfBoundsException if {@link #writerIndex()} + {@code minWritableBytes} > {@link #maxCapacity()}
     */
    @Override
    public ByteBuf ensureWritable(int minWritableBytes) {
        return parent.ensureWritable(minWritableBytes);
    }

    /**
     * Tries to make sure the number of {@linkplain #writableBytes() the writable bytes}
     * is equal to or greater than the specified value.  Unlike {@link #ensureWritable(int)},
     * this method does not raise an exception but returns a code.
     *
     * @param minWritableBytes the expected minimum number of writable bytes
     * @param force When {@link #writerIndex()} + {@code minWritableBytes} > {@link #maxCapacity()}:
     * <ul>
     * <li>{@code true} - the capacity of the buffer is expanded to {@link #maxCapacity()}</li>
     * <li>{@code false} - the capacity of the buffer is unchanged</li>
     * </ul>
     * @return {@code 0} if the buffer has enough writable bytes, and its capacity is unchanged.
     * {@code 1} if the buffer does not have enough bytes, and its capacity is unchanged.
     * {@code 2} if the buffer has enough writable bytes, and its capacity has been increased.
     * {@code 3} if the buffer does not have enough bytes, but its capacity has been
     * increased to its maximum.
     */
    @Override
    public int ensureWritable(int minWritableBytes, boolean force) {
        return parent.ensureWritable(minWritableBytes, force);
    }

    /**
     * Gets a boolean at the specified absolute (@code index) in this buffer.
     * This method does not modify the {@code readerIndex} or {@code writerIndex}
     * of this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * {@code index + 1} is greater than {@code this.capacity}
     */
    @Override
    public boolean getBoolean(int index) {
        return parent.getBoolean(index);
    }

    /**
     * Gets a byte at the specified absolute {@code index} in this buffer.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * {@code index + 1} is greater than {@code this.capacity}
     */
    @Override
    public byte getByte(int index) {
        return parent.getByte(index);
    }

    /**
     * Gets an unsigned byte at the specified absolute {@code index} in this
     * buffer.  This method does not modify {@code readerIndex} or
     * {@code writerIndex} of this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * {@code index + 1} is greater than {@code this.capacity}
     */
    @Override
    public short getUnsignedByte(int index) {
        return parent.getUnsignedByte(index);
    }

    /**
     * Gets a 16-bit short integer at the specified absolute {@code index} in
     * this buffer.  This method does not modify {@code readerIndex} or
     * {@code writerIndex} of this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * {@code index + 2} is greater than {@code this.capacity}
     */
    @Override
    public short getShort(int index) {
        return parent.getShort(index);
    }

    /**
     * Gets an unsigned 16-bit short integer at the specified absolute
     * {@code index} in this buffer.  This method does not modify
     * {@code readerIndex} or {@code writerIndex} of this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * {@code index + 2} is greater than {@code this.capacity}
     */
    @Override
    public int getUnsignedShort(int index) {
        return parent.getUnsignedShort(index);
    }

    /**
     * Gets a 24-bit medium integer at the specified absolute {@code index} in
     * this buffer.  This method does not modify {@code readerIndex} or
     * {@code writerIndex} of this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * {@code index + 3} is greater than {@code this.capacity}
     */
    @Override
    public int getMedium(int index) {
        return parent.getMedium(index);
    }

    /**
     * Gets an unsigned 24-bit medium integer at the specified absolute
     * {@code index} in this buffer.  This method does not modify
     * {@code readerIndex} or {@code writerIndex} of this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * {@code index + 3} is greater than {@code this.capacity}
     */
    @Override
    public int getUnsignedMedium(int index) {
        return parent.getUnsignedMedium(index);
    }

    /**
     * Gets a 32-bit integer at the specified absolute {@code index} in
     * this buffer.  This method does not modify {@code readerIndex} or
     * {@code writerIndex} of this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * {@code index + 4} is greater than {@code this.capacity}
     */
    @Override
    public int getInt(int index) {
        return parent.getInt(index);
    }

    /**
     * Gets an unsigned 32-bit integer at the specified absolute {@code index}
     * in this buffer.  This method does not modify {@code readerIndex} or
     * {@code writerIndex} of this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * {@code index + 4} is greater than {@code this.capacity}
     */
    @Override
    public long getUnsignedInt(int index) {
        return parent.getUnsignedInt(index);
    }

    /**
     * Gets a 64-bit long integer at the specified absolute {@code index} in
     * this buffer.  This method does not modify {@code readerIndex} or
     * {@code writerIndex} of this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * {@code index + 8} is greater than {@code this.capacity}
     */
    @Override
    public long getLong(int index) {
        return parent.getLong(index);
    }

    /**
     * Gets a 2-byte UTF-16 character at the specified absolute
     * {@code index} in this buffer.  This method does not modify
     * {@code readerIndex} or {@code writerIndex} of this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * {@code index + 2} is greater than {@code this.capacity}
     */
    @Override
    public char getChar(int index) {
        return parent.getChar(index);
    }

    /**
     * Gets a 32-bit floating point number at the specified absolute
     * {@code index} in this buffer.  This method does not modify
     * {@code readerIndex} or {@code writerIndex} of this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * {@code index + 4} is greater than {@code this.capacity}
     */
    @Override
    public float getFloat(int index) {
        return parent.getFloat(index);
    }

    /**
     * Gets a 64-bit floating point number at the specified absolute
     * {@code index} in this buffer.  This method does not modify
     * {@code readerIndex} or {@code writerIndex} of this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * {@code index + 8} is greater than {@code this.capacity}
     */
    @Override
    public double getDouble(int index) {
        return parent.getDouble(index);
    }

    /**
     * Transfers this buffer's data to the specified destination starting at
     * the specified absolute {@code index} until the destination becomes
     * non-writable.  This method is basically same with
     * {@link #getBytes(int, ByteBuf, int, int)}, except that this
     * method increases the {@code writerIndex} of the destination by the
     * number of the transferred bytes while
     * {@link #getBytes(int, ByteBuf, int, int)} does not.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * the source buffer (i.e. {@code this}).
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * if {@code index + dst.writableBytes} is greater than
     * {@code this.capacity}
     */
    @Override
    public ByteBuf getBytes(int index, ByteBuf dst) {
        return parent.getBytes(index, dst);
    }

    /**
     * Transfers this buffer's data to the specified destination starting at
     * the specified absolute {@code index}.  This method is basically same
     * with {@link #getBytes(int, ByteBuf, int, int)}, except that this
     * method increases the {@code writerIndex} of the destination by the
     * number of the transferred bytes while
     * {@link #getBytes(int, ByteBuf, int, int)} does not.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * the source buffer (i.e. {@code this}).
     *
     * @param length the number of bytes to transfer
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0},
     * if {@code index + length} is greater than
     * {@code this.capacity}, or
     * if {@code length} is greater than {@code dst.writableBytes}
     */
    @Override
    public ByteBuf getBytes(int index, ByteBuf dst, int length) {
        return parent.getBytes(index, dst, length);
    }

    /**
     * Transfers this buffer's data to the specified destination starting at
     * the specified absolute {@code index}.
     * This method does not modify {@code readerIndex} or {@code writerIndex}
     * of both the source (i.e. {@code this}) and the destination.
     *
     * @param dstIndex the first index of the destination
     * @param length the number of bytes to transfer
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0},
     * if the specified {@code dstIndex} is less than {@code 0},
     * if {@code index + length} is greater than
     * {@code this.capacity}, or
     * if {@code dstIndex + length} is greater than
     * {@code dst.capacity}
     */
    @Override
    public ByteBuf getBytes(int index, ByteBuf dst, int dstIndex, int length) {
        return parent.getBytes(index, dst, dstIndex, length);
    }

    /**
     * Transfers this buffer's data to the specified destination starting at
     * the specified absolute {@code index}.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * if {@code index + dst.length} is greater than
     * {@code this.capacity}
     */
    @Override
    public ByteBuf getBytes(int index, byte[] dst) {
        return parent.getBytes(index, dst);
    }

    /**
     * Transfers this buffer's data to the specified destination starting at
     * the specified absolute {@code index}.
     * This method does not modify {@code readerIndex} or {@code writerIndex}
     * of this buffer.
     *
     * @param dstIndex the first index of the destination
     * @param length the number of bytes to transfer
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0},
     * if the specified {@code dstIndex} is less than {@code 0},
     * if {@code index + length} is greater than
     * {@code this.capacity}, or
     * if {@code dstIndex + length} is greater than
     * {@code dst.length}
     */
    @Override
    public ByteBuf getBytes(int index, byte[] dst, int dstIndex, int length) {
        return parent.getBytes(index, dst, dstIndex, length);
    }

    /**
     * Transfers this buffer's data to the specified destination starting at
     * the specified absolute {@code index} until the destination's position
     * reaches its limit.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer while the destination's {@code position} will be increased.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * if {@code index + dst.remaining()} is greater than
     * {@code this.capacity}
     */
    @Override
    public ByteBuf getBytes(int index, ByteBuffer dst) {
        return parent.getBytes(index, dst);
    }

    /**
     * Transfers this buffer's data to the specified stream starting at the
     * specified absolute {@code index}.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @param length the number of bytes to transfer
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * if {@code index + length} is greater than
     * {@code this.capacity}
     * @throws IOException if the specified stream threw an exception during I/O
     */
    @Override
    public ByteBuf getBytes(int index, OutputStream out, int length) throws IOException {
        return parent.getBytes(index, out, length);
    }

    /**
     * Transfers this buffer's data to the specified channel starting at the
     * specified absolute {@code index}.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @param length the maximum number of bytes to transfer
     * @return the actual number of bytes written out to the specified channel
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * if {@code index + length} is greater than
     * {@code this.capacity}
     * @throws IOException if the specified channel threw an exception during I/O
     */
    @Override
    public int getBytes(int index, GatheringByteChannel out, int length) throws IOException {
        return parent.getBytes(index, out, length);
    }

    /**
     * Sets the specified boolean at the specified absolute {@code index} in this
     * buffer.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * {@code index + 1} is greater than {@code this.capacity}
     */
    @Override
    public ByteBuf setBoolean(int index, boolean value) {
        return parent.setBoolean(index, value);
    }

    /**
     * Sets the specified byte at the specified absolute {@code index} in this
     * buffer.  The 24 high-order bits of the specified value are ignored.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * {@code index + 1} is greater than {@code this.capacity}
     */
    @Override
    public ByteBuf setByte(int index, int value) {
        return parent.setByte(index, value);
    }

    /**
     * Sets the specified 16-bit short integer at the specified absolute
     * {@code index} in this buffer.  The 16 high-order bits of the specified
     * value are ignored.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * {@code index + 2} is greater than {@code this.capacity}
     */
    @Override
    public ByteBuf setShort(int index, int value) {
        return parent.setShort(index, value);
    }

    /**
     * Sets the specified 24-bit medium integer at the specified absolute
     * {@code index} in this buffer.  Please take in account that the most significant
     * byte is ignored in the specified value.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * {@code index + 3} is greater than {@code this.capacity}
     */
    @Override
    public ByteBuf setMedium(int index, int value) {
        return parent.setMedium(index, value);
    }

    /**
     * Sets the specified 32-bit integer at the specified absolute
     * {@code index} in this buffer.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * {@code index + 4} is greater than {@code this.capacity}
     */
    @Override
    public ByteBuf setInt(int index, int value) {
        return parent.setInt(index, value);
    }

    /**
     * Sets the specified 64-bit long integer at the specified absolute
     * {@code index} in this buffer.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * {@code index + 8} is greater than {@code this.capacity}
     */
    @Override
    public ByteBuf setLong(int index, long value) {
        return parent.setLong(index, value);
    }

    /**
     * Sets the specified 2-byte UTF-16 character at the specified absolute
     * {@code index} in this buffer.
     * The 16 high-order bits of the specified value are ignored.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * {@code index + 2} is greater than {@code this.capacity}
     */
    @Override
    public ByteBuf setChar(int index, int value) {
        return parent.setChar(index, value);
    }

    /**
     * Sets the specified 32-bit floating-point number at the specified
     * absolute {@code index} in this buffer.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * {@code index + 4} is greater than {@code this.capacity}
     */
    @Override
    public ByteBuf setFloat(int index, float value) {
        return parent.setFloat(index, value);
    }

    /**
     * Sets the specified 64-bit floating-point number at the specified
     * absolute {@code index} in this buffer.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * {@code index + 8} is greater than {@code this.capacity}
     */
    @Override
    public ByteBuf setDouble(int index, double value) {
        return parent.setDouble(index, value);
    }

    /**
     * Transfers the specified source buffer's data to this buffer starting at
     * the specified absolute {@code index} until the source buffer becomes
     * unreadable.  This method is basically same with
     * {@link #setBytes(int, ByteBuf, int, int)}, except that this
     * method increases the {@code readerIndex} of the source buffer by
     * the number of the transferred bytes while
     * {@link #setBytes(int, ByteBuf, int, int)} does not.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * the source buffer (i.e. {@code this}).
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * if {@code index + src.readableBytes} is greater than
     * {@code this.capacity}
     */
    @Override
    public ByteBuf setBytes(int index, ByteBuf src) {
        return parent.setBytes(index, src);
    }

    /**
     * Transfers the specified source buffer's data to this buffer starting at
     * the specified absolute {@code index}.  This method is basically same
     * with {@link #setBytes(int, ByteBuf, int, int)}, except that this
     * method increases the {@code readerIndex} of the source buffer by
     * the number of the transferred bytes while
     * {@link #setBytes(int, ByteBuf, int, int)} does not.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * the source buffer (i.e. {@code this}).
     *
     * @param length the number of bytes to transfer
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0},
     * if {@code index + length} is greater than
     * {@code this.capacity}, or
     * if {@code length} is greater than {@code src.readableBytes}
     */
    @Override
    public ByteBuf setBytes(int index, ByteBuf src, int length) {
        return parent.setBytes(index, src, length);
    }

    /**
     * Transfers the specified source buffer's data to this buffer starting at
     * the specified absolute {@code index}.
     * This method does not modify {@code readerIndex} or {@code writerIndex}
     * of both the source (i.e. {@code this}) and the destination.
     *
     * @param srcIndex the first index of the source
     * @param length the number of bytes to transfer
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0},
     * if the specified {@code srcIndex} is less than {@code 0},
     * if {@code index + length} is greater than
     * {@code this.capacity}, or
     * if {@code srcIndex + length} is greater than
     * {@code src.capacity}
     */
    @Override
    public ByteBuf setBytes(int index, ByteBuf src, int srcIndex, int length) {
        return parent.setBytes(index, src, srcIndex, length);
    }

    /**
     * Transfers the specified source array's data to this buffer starting at
     * the specified absolute {@code index}.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * if {@code index + src.length} is greater than
     * {@code this.capacity}
     */
    @Override
    public ByteBuf setBytes(int index, byte[] src) {
        return parent.setBytes(index, src);
    }

    /**
     * Transfers the specified source array's data to this buffer starting at
     * the specified absolute {@code index}.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0},
     * if the specified {@code srcIndex} is less than {@code 0},
     * if {@code index + length} is greater than
     * {@code this.capacity}, or
     * if {@code srcIndex + length} is greater than {@code src.length}
     */
    @Override
    public ByteBuf setBytes(int index, byte[] src, int srcIndex, int length) {
        return parent.setBytes(index, src, srcIndex, length);
    }

    /**
     * Transfers the specified source buffer's data to this buffer starting at
     * the specified absolute {@code index} until the source buffer's position
     * reaches its limit.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * if {@code index + src.remaining()} is greater than
     * {@code this.capacity}
     */
    @Override
    public ByteBuf setBytes(int index, ByteBuffer src) {
        return parent.setBytes(index, src);
    }

    /**
     * Transfers the content of the specified source stream to this buffer
     * starting at the specified absolute {@code index}.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @param length the number of bytes to transfer
     * @return the actual number of bytes read in from the specified channel.
     * {@code -1} if the specified channel is closed.
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * if {@code index + length} is greater than {@code this.capacity}
     * @throws IOException if the specified stream threw an exception during I/O
     */
    @Override
    public int setBytes(int index, InputStream in, int length) throws IOException {
        return parent.setBytes(index, in, length);
    }

    /**
     * Transfers the content of the specified source channel to this buffer
     * starting at the specified absolute {@code index}.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @param length the maximum number of bytes to transfer
     * @return the actual number of bytes read in from the specified channel.
     * {@code -1} if the specified channel is closed.
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * if {@code index + length} is greater than {@code this.capacity}
     * @throws IOException if the specified channel threw an exception during I/O
     */
    @Override
    public int setBytes(int index, ScatteringByteChannel in, int length) throws IOException {
        return parent.setBytes(index, in, length);
    }

    /**
     * Fills this buffer with <tt>NUL (0x00)</tt> starting at the specified
     * absolute {@code index}.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @param length the number of <tt>NUL</tt>s to write to the buffer
     * @throws IndexOutOfBoundsException if the specified {@code index} is less than {@code 0} or
     * if {@code index + length} is greater than {@code this.capacity}
     */
    @Override
    public ByteBuf setZero(int index, int length) {
        return parent.setZero(index, length);
    }

    /**
     * Gets a boolean at the current {@code readerIndex} and increases
     * the {@code readerIndex} by {@code 1} in this buffer.
     *
     * @throws IndexOutOfBoundsException if {@code this.readableBytes} is less than {@code 1}
     */
    @Override
    public boolean readBoolean() {
        return parent.readBoolean();
    }

    /**
     * Gets a byte at the current {@code readerIndex} and increases
     * the {@code readerIndex} by {@code 1} in this buffer.
     *
     * @throws IndexOutOfBoundsException if {@code this.readableBytes} is less than {@code 1}
     */
    @Override
    public byte readByte() {
        return parent.readByte();
    }

    /**
     * Gets an unsigned byte at the current {@code readerIndex} and increases
     * the {@code readerIndex} by {@code 1} in this buffer.
     *
     * @throws IndexOutOfBoundsException if {@code this.readableBytes} is less than {@code 1}
     */
    @Override
    public short readUnsignedByte() {
        return parent.readUnsignedByte();
    }

    /**
     * Gets a 16-bit short integer at the current {@code readerIndex}
     * and increases the {@code readerIndex} by {@code 2} in this buffer.
     *
     * @throws IndexOutOfBoundsException if {@code this.readableBytes} is less than {@code 2}
     */
    @Override
    public short readShort() {
        return parent.readShort();
    }

    /**
     * Gets an unsigned 16-bit short integer at the current {@code readerIndex}
     * and increases the {@code readerIndex} by {@code 2} in this buffer.
     *
     * @throws IndexOutOfBoundsException if {@code this.readableBytes} is less than {@code 2}
     */
    @Override
    public int readUnsignedShort() {
        return parent.readUnsignedShort();
    }

    /**
     * Gets a 24-bit medium integer at the current {@code readerIndex}
     * and increases the {@code readerIndex} by {@code 3} in this buffer.
     *
     * @throws IndexOutOfBoundsException if {@code this.readableBytes} is less than {@code 3}
     */
    @Override
    public int readMedium() {
        return parent.readMedium();
    }

    /**
     * Gets an unsigned 24-bit medium integer at the current {@code readerIndex}
     * and increases the {@code readerIndex} by {@code 3} in this buffer.
     *
     * @throws IndexOutOfBoundsException if {@code this.readableBytes} is less than {@code 3}
     */
    @Override
    public int readUnsignedMedium() {
        return parent.readUnsignedMedium();
    }

    /**
     * Gets a 32-bit integer at the current {@code readerIndex}
     * and increases the {@code readerIndex} by {@code 4} in this buffer.
     *
     * @throws IndexOutOfBoundsException if {@code this.readableBytes} is less than {@code 4}
     */
    @Override
    public int readInt() {
        return parent.readInt();
    }

    /**
     * Gets an unsigned 32-bit integer at the current {@code readerIndex}
     * and increases the {@code readerIndex} by {@code 4} in this buffer.
     *
     * @throws IndexOutOfBoundsException if {@code this.readableBytes} is less than {@code 4}
     */
    @Override
    public long readUnsignedInt() {
        return parent.readUnsignedInt();
    }

    /**
     * Gets a 64-bit integer at the current {@code readerIndex}
     * and increases the {@code readerIndex} by {@code 8} in this buffer.
     *
     * @throws IndexOutOfBoundsException if {@code this.readableBytes} is less than {@code 8}
     */
    @Override
    public long readLong() {
        return parent.readLong();
    }

    /**
     * Gets a 2-byte UTF-16 character at the current {@code readerIndex}
     * and increases the {@code readerIndex} by {@code 2} in this buffer.
     *
     * @throws IndexOutOfBoundsException if {@code this.readableBytes} is less than {@code 2}
     */
    @Override
    public char readChar() {
        return parent.readChar();
    }

    /**
     * Gets a 32-bit floating point number at the current {@code readerIndex}
     * and increases the {@code readerIndex} by {@code 4} in this buffer.
     *
     * @throws IndexOutOfBoundsException if {@code this.readableBytes} is less than {@code 4}
     */
    @Override
    public float readFloat() {
        return parent.readFloat();
    }

    /**
     * Gets a 64-bit floating point number at the current {@code readerIndex}
     * and increases the {@code readerIndex} by {@code 8} in this buffer.
     *
     * @throws IndexOutOfBoundsException if {@code this.readableBytes} is less than {@code 8}
     */
    @Override
    public double readDouble() {
        return parent.readDouble();
    }

    /**
     * Transfers this buffer's data to a newly created buffer starting at
     * the current {@code readerIndex} and increases the {@code readerIndex}
     * by the number of the transferred bytes (= {@code length}).
     * The returned buffer's {@code readerIndex} and {@code writerIndex} are
     * {@code 0} and {@code length} respectively.
     *
     * @param length the number of bytes to transfer
     * @return the newly created buffer which contains the transferred bytes
     * @throws IndexOutOfBoundsException if {@code length} is greater than {@code this.readableBytes}
     */
    @Override
    public ByteBuf readBytes(int length) {
        return parent.readBytes(length);
    }

    /**
     * Returns a new slice of this buffer's sub-region starting at the current
     * {@code readerIndex} and increases the {@code readerIndex} by the size
     * of the new slice (= {@code length}).
     *
     * @param length the size of the new slice
     * @return the newly created slice
     * @throws IndexOutOfBoundsException if {@code length} is greater than {@code this.readableBytes}
     */
    @Override
    public ByteBuf readSlice(int length) {
        return parent.readSlice(length);
    }

    /**
     * Transfers this buffer's data to the specified destination starting at
     * the current {@code readerIndex} until the destination becomes
     * non-writable, and increases the {@code readerIndex} by the number of the
     * transferred bytes.  This method is basically same with
     * {@link #readBytes(ByteBuf, int, int)}, except that this method
     * increases the {@code writerIndex} of the destination by the number of
     * the transferred bytes while {@link #readBytes(ByteBuf, int, int)}
     * does not.
     *
     * @throws IndexOutOfBoundsException if {@code dst.writableBytes} is greater than
     * {@code this.readableBytes}
     */
    @Override
    public ByteBuf readBytes(ByteBuf dst) {
        return parent.readBytes(dst);
    }

    /**
     * Transfers this buffer's data to the specified destination starting at
     * the current {@code readerIndex} and increases the {@code readerIndex}
     * by the number of the transferred bytes (= {@code length}).  This method
     * is basically same with {@link #readBytes(ByteBuf, int, int)},
     * except that this method increases the {@code writerIndex} of the
     * destination by the number of the transferred bytes (= {@code length})
     * while {@link #readBytes(ByteBuf, int, int)} does not.
     *
     * @throws IndexOutOfBoundsException if {@code length} is greater than {@code this.readableBytes} or
     * if {@code length} is greater than {@code dst.writableBytes}
     */
    @Override
    public ByteBuf readBytes(ByteBuf dst, int length) {
        return parent.readBytes(dst, length);
    }

    /**
     * Transfers this buffer's data to the specified destination starting at
     * the current {@code readerIndex} and increases the {@code readerIndex}
     * by the number of the transferred bytes (= {@code length}).
     *
     * @param dstIndex the first index of the destination
     * @param length the number of bytes to transfer
     * @throws IndexOutOfBoundsException if the specified {@code dstIndex} is less than {@code 0},
     * if {@code length} is greater than {@code this.readableBytes}, or
     * if {@code dstIndex + length} is greater than
     * {@code dst.capacity}
     */
    @Override
    public ByteBuf readBytes(ByteBuf dst, int dstIndex, int length) {
        return parent.readBytes(dst, dstIndex, length);
    }

    /**
     * Transfers this buffer's data to the specified destination starting at
     * the current {@code readerIndex} and increases the {@code readerIndex}
     * by the number of the transferred bytes (= {@code dst.length}).
     *
     * @throws IndexOutOfBoundsException if {@code dst.length} is greater than {@code this.readableBytes}
     */
    @Override
    public ByteBuf readBytes(byte[] dst) {
        return parent.readBytes(dst);
    }

    /**
     * Transfers this buffer's data to the specified destination starting at
     * the current {@code readerIndex} and increases the {@code readerIndex}
     * by the number of the transferred bytes (= {@code length}).
     *
     * @param dstIndex the first index of the destination
     * @param length the number of bytes to transfer
     * @throws IndexOutOfBoundsException if the specified {@code dstIndex} is less than {@code 0},
     * if {@code length} is greater than {@code this.readableBytes}, or
     * if {@code dstIndex + length} is greater than {@code dst.length}
     */
    @Override
    public ByteBuf readBytes(byte[] dst, int dstIndex, int length) {
        return parent.readBytes(dst, dstIndex, length);
    }

    /**
     * Transfers this buffer's data to the specified destination starting at
     * the current {@code readerIndex} until the destination's position
     * reaches its limit, and increases the {@code readerIndex} by the
     * number of the transferred bytes.
     *
     * @throws IndexOutOfBoundsException if {@code dst.remaining()} is greater than
     * {@code this.readableBytes}
     */
    @Override
    public ByteBuf readBytes(ByteBuffer dst) {
        return parent.readBytes(dst);
    }

    /**
     * Transfers this buffer's data to the specified stream starting at the
     * current {@code readerIndex}.
     *
     * @param length the number of bytes to transfer
     * @throws IndexOutOfBoundsException if {@code length} is greater than {@code this.readableBytes}
     * @throws IOException if the specified stream threw an exception during I/O
     */
    @Override
    public ByteBuf readBytes(OutputStream out, int length) throws IOException {
        return parent.readBytes(out, length);
    }

    /**
     * Transfers this buffer's data to the specified stream starting at the
     * current {@code readerIndex}.
     *
     * @param length the maximum number of bytes to transfer
     * @return the actual number of bytes written out to the specified channel
     * @throws IndexOutOfBoundsException if {@code length} is greater than {@code this.readableBytes}
     * @throws IOException if the specified channel threw an exception during I/O
     */
    @Override
    public int readBytes(GatheringByteChannel out, int length) throws IOException {
        return parent.readBytes(out, length);
    }

    /**
     * Increases the current {@code readerIndex} by the specified
     * {@code length} in this buffer.
     *
     * @throws IndexOutOfBoundsException if {@code length} is greater than {@code this.readableBytes}
     */
    @Override
    public ByteBuf skipBytes(int length) {
        return parent.skipBytes(length);
    }

    /**
     * Sets the specified boolean at the current {@code writerIndex}
     * and increases the {@code writerIndex} by {@code 1} in this buffer.
     *
     * @throws IndexOutOfBoundsException if {@code this.writableBytes} is less than {@code 1}
     */
    @Override
    public ByteBuf writeBoolean(boolean value) {
        return parent.writeBoolean(value);
    }

    /**
     * Sets the specified byte at the current {@code writerIndex}
     * and increases the {@code writerIndex} by {@code 1} in this buffer.
     * The 24 high-order bits of the specified value are ignored.
     *
     * @throws IndexOutOfBoundsException if {@code this.writableBytes} is less than {@code 1}
     */
    @Override
    public ByteBuf writeByte(int value) {
        return parent.writeByte(value);
    }

    /**
     * Sets the specified 16-bit short integer at the current
     * {@code writerIndex} and increases the {@code writerIndex} by {@code 2}
     * in this buffer.  The 16 high-order bits of the specified value are ignored.
     *
     * @throws IndexOutOfBoundsException if {@code this.writableBytes} is less than {@code 2}
     */
    @Override
    public ByteBuf writeShort(int value) {
        return parent.writeShort(value);
    }

    /**
     * Sets the specified 24-bit medium integer at the current
     * {@code writerIndex} and increases the {@code writerIndex} by {@code 3}
     * in this buffer.
     *
     * @throws IndexOutOfBoundsException if {@code this.writableBytes} is less than {@code 3}
     */
    @Override
    public ByteBuf writeMedium(int value) {
        return parent.writeMedium(value);
    }

    /**
     * Sets the specified 32-bit integer at the current {@code writerIndex}
     * and increases the {@code writerIndex} by {@code 4} in this buffer.
     *
     * @throws IndexOutOfBoundsException if {@code this.writableBytes} is less than {@code 4}
     */
    @Override
    public ByteBuf writeInt(int value) {
        return parent.writeInt(value);
    }

    /**
     * Sets the specified 64-bit long integer at the current
     * {@code writerIndex} and increases the {@code writerIndex} by {@code 8}
     * in this buffer.
     *
     * @throws IndexOutOfBoundsException if {@code this.writableBytes} is less than {@code 8}
     */
    @Override
    public ByteBuf writeLong(long value) {
        return parent.writeLong(value);
    }

    /**
     * Sets the specified 2-byte UTF-16 character at the current
     * {@code writerIndex} and increases the {@code writerIndex} by {@code 2}
     * in this buffer.  The 16 high-order bits of the specified value are ignored.
     *
     * @throws IndexOutOfBoundsException if {@code this.writableBytes} is less than {@code 2}
     */
    @Override
    public ByteBuf writeChar(int value) {
        return parent.writeChar(value);
    }

    /**
     * Sets the specified 32-bit floating point number at the current
     * {@code writerIndex} and increases the {@code writerIndex} by {@code 4}
     * in this buffer.
     *
     * @throws IndexOutOfBoundsException if {@code this.writableBytes} is less than {@code 4}
     */
    @Override
    public ByteBuf writeFloat(float value) {
        return parent.writeFloat(value);
    }

    /**
     * Sets the specified 64-bit floating point number at the current
     * {@code writerIndex} and increases the {@code writerIndex} by {@code 8}
     * in this buffer.
     *
     * @throws IndexOutOfBoundsException if {@code this.writableBytes} is less than {@code 8}
     */
    @Override
    public ByteBuf writeDouble(double value) {
        return parent.writeDouble(value);
    }

    /**
     * Transfers the specified source buffer's data to this buffer starting at
     * the current {@code writerIndex} until the source buffer becomes
     * unreadable, and increases the {@code writerIndex} by the number of
     * the transferred bytes.  This method is basically same with
     * {@link #writeBytes(ByteBuf, int, int)}, except that this method
     * increases the {@code readerIndex} of the source buffer by the number of
     * the transferred bytes while {@link #writeBytes(ByteBuf, int, int)}
     * does not.
     *
     * @throws IndexOutOfBoundsException if {@code src.readableBytes} is greater than
     * {@code this.writableBytes}
     */
    @Override
    public ByteBuf writeBytes(ByteBuf src) {
        return parent.writeBytes(src);
    }

    /**
     * Transfers the specified source buffer's data to this buffer starting at
     * the current {@code writerIndex} and increases the {@code writerIndex}
     * by the number of the transferred bytes (= {@code length}).  This method
     * is basically same with {@link #writeBytes(ByteBuf, int, int)},
     * except that this method increases the {@code readerIndex} of the source
     * buffer by the number of the transferred bytes (= {@code length}) while
     * {@link #writeBytes(ByteBuf, int, int)} does not.
     *
     * @param length the number of bytes to transfer
     * @throws IndexOutOfBoundsException if {@code length} is greater than {@code this.writableBytes} or
     * if {@code length} is greater then {@code src.readableBytes}
     */
    @Override
    public ByteBuf writeBytes(ByteBuf src, int length) {
        return parent.writeBytes(src, length);
    }

    /**
     * Transfers the specified source buffer's data to this buffer starting at
     * the current {@code writerIndex} and increases the {@code writerIndex}
     * by the number of the transferred bytes (= {@code length}).
     *
     * @param srcIndex the first index of the source
     * @param length the number of bytes to transfer
     * @throws IndexOutOfBoundsException if the specified {@code srcIndex} is less than {@code 0},
     * if {@code srcIndex + length} is greater than
     * {@code src.capacity}, or
     * if {@code length} is greater than {@code this.writableBytes}
     */
    @Override
    public ByteBuf writeBytes(ByteBuf src, int srcIndex, int length) {
        return parent.writeBytes(src, srcIndex, length);
    }

    /**
     * Transfers the specified source array's data to this buffer starting at
     * the current {@code writerIndex} and increases the {@code writerIndex}
     * by the number of the transferred bytes (= {@code src.length}).
     *
     * @throws IndexOutOfBoundsException if {@code src.length} is greater than {@code this.writableBytes}
     */
    @Override
    public ByteBuf writeBytes(byte[] src) {
        return parent.writeBytes(src);
    }

    /**
     * Transfers the specified source array's data to this buffer starting at
     * the current {@code writerIndex} and increases the {@code writerIndex}
     * by the number of the transferred bytes (= {@code length}).
     *
     * @param srcIndex the first index of the source
     * @param length the number of bytes to transfer
     * @throws IndexOutOfBoundsException if the specified {@code srcIndex} is less than {@code 0},
     * if {@code srcIndex + length} is greater than
     * {@code src.length}, or
     * if {@code length} is greater than {@code this.writableBytes}
     */
    @Override
    public ByteBuf writeBytes(byte[] src, int srcIndex, int length) {
        return parent.writeBytes(src, srcIndex, length);
    }

    /**
     * Transfers the specified source buffer's data to this buffer starting at
     * the current {@code writerIndex} until the source buffer's position
     * reaches its limit, and increases the {@code writerIndex} by the
     * number of the transferred bytes.
     *
     * @throws IndexOutOfBoundsException if {@code src.remaining()} is greater than
     * {@code this.writableBytes}
     */
    @Override
    public ByteBuf writeBytes(ByteBuffer src) {
        return parent.writeBytes(src);
    }

    /**
     * Transfers the content of the specified stream to this buffer
     * starting at the current {@code writerIndex} and increases the
     * {@code writerIndex} by the number of the transferred bytes.
     *
     * @param length the number of bytes to transfer
     * @return the actual number of bytes read in from the specified stream
     * @throws IndexOutOfBoundsException if {@code length} is greater than {@code this.writableBytes}
     * @throws IOException if the specified stream threw an exception during I/O
     */
    @Override
    public int writeBytes(InputStream in, int length) throws IOException {
        return parent.writeBytes(in, length);
    }

    /**
     * Transfers the content of the specified channel to this buffer
     * starting at the current {@code writerIndex} and increases the
     * {@code writerIndex} by the number of the transferred bytes.
     *
     * @param length the maximum number of bytes to transfer
     * @return the actual number of bytes read in from the specified channel
     * @throws IndexOutOfBoundsException if {@code length} is greater than {@code this.writableBytes}
     * @throws IOException if the specified channel threw an exception during I/O
     */
    @Override
    public int writeBytes(ScatteringByteChannel in, int length) throws IOException {
        return parent.writeBytes(in, length);
    }

    /**
     * Fills this buffer with <tt>NUL (0x00)</tt> starting at the current
     * {@code writerIndex} and increases the {@code writerIndex} by the
     * specified {@code length}.
     *
     * @param length the number of <tt>NUL</tt>s to write to the buffer
     * @throws IndexOutOfBoundsException if {@code length} is greater than {@code this.writableBytes}
     */
    @Override
    public ByteBuf writeZero(int length) {
        return parent.writeZero(length);
    }

    /**
     * Locates the first occurrence of the specified {@code value} in this
     * buffer.  The search takes place from the specified {@code fromIndex}
     * (inclusive)  to the specified {@code toIndex} (exclusive).
     * <p>
     * If {@code fromIndex} is greater than {@code toIndex}, the search is
     * performed in a reversed order.
     * <p>
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @return the absolute index of the first occurrence if found.
     * {@code -1} otherwise.
     */
    @Override
    public int indexOf(int fromIndex, int toIndex, byte value) {
        return parent.indexOf(fromIndex, toIndex, value);
    }

    /**
     * Locates the first occurrence of the specified {@code value} in this
     * buffer.  The search takes place from the current {@code readerIndex}
     * (inclusive) to the current {@code writerIndex} (exclusive).
     * <p>
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @return the number of bytes between the current {@code readerIndex}
     * and the first occurrence if found. {@code -1} otherwise.
     */
    @Override
    public int bytesBefore(byte value) {
        return parent.bytesBefore(value);
    }

    /**
     * Locates the first occurrence of the specified {@code value} in this
     * buffer.  The search starts from the current {@code readerIndex}
     * (inclusive) and lasts for the specified {@code length}.
     * <p>
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @return the number of bytes between the current {@code readerIndex}
     * and the first occurrence if found. {@code -1} otherwise.
     * @throws IndexOutOfBoundsException if {@code length} is greater than {@code this.readableBytes}
     */
    @Override
    public int bytesBefore(int length, byte value) {
        return parent.bytesBefore(length, value);
    }

    /**
     * Locates the first occurrence of the specified {@code value} in this
     * buffer.  The search starts from the specified {@code index} (inclusive)
     * and lasts for the specified {@code length}.
     * <p>
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @return the number of bytes between the specified {@code index}
     * and the first occurrence if found. {@code -1} otherwise.
     * @throws IndexOutOfBoundsException if {@code index + length} is greater than {@code this.capacity}
     */
    @Override
    public int bytesBefore(int index, int length, byte value) {
        return parent.bytesBefore(index, length, value);
    }

    /**
     * Iterates over the readable bytes of this buffer with the specified {@code processor} in ascending order.
     *
     * @return {@code -1} if the processor iterated to or beyond the end of the readable bytes.
     * The last-visited index If the {@link ByteBufProcessor#process(byte)} returned {@code false}.
     */
    @Override
    public int forEachByte(ByteBufProcessor processor) {
        return parent.forEachByte(processor);
    }

    /**
     * Iterates over the specified area of this buffer with the specified {@code processor} in ascending order.
     * (i.e. {@code index}, {@code (index + 1)},  .. {@code (index + length - 1)})
     *
     * @return {@code -1} if the processor iterated to or beyond the end of the specified area.
     * The last-visited index If the {@link ByteBufProcessor#process(byte)} returned {@code false}.
     */
    @Override
    public int forEachByte(int index, int length, ByteBufProcessor processor) {
        return parent.forEachByte(index, length, processor);
    }

    /**
     * Iterates over the readable bytes of this buffer with the specified {@code processor} in descending order.
     *
     * @return {@code -1} if the processor iterated to or beyond the beginning of the readable bytes.
     * The last-visited index If the {@link ByteBufProcessor#process(byte)} returned {@code false}.
     */
    @Override
    public int forEachByteDesc(ByteBufProcessor processor) {
        return parent.forEachByteDesc(processor);
    }

    /**
     * Iterates over the specified area of this buffer with the specified {@code processor} in descending order.
     * (i.e. {@code (index + length - 1)}, {@code (index + length - 2)}, ... {@code index})
     *
     * @return {@code -1} if the processor iterated to or beyond the beginning of the specified area.
     * The last-visited index If the {@link ByteBufProcessor#process(byte)} returned {@code false}.
     */
    @Override
    public int forEachByteDesc(int index, int length, ByteBufProcessor processor) {
        return parent.forEachByteDesc(index, length, processor);
    }

    /**
     * Returns a copy of this buffer's readable bytes.  Modifying the content
     * of the returned buffer or this buffer does not affect each other at all.
     * This method is identical to {@code buf.copy(buf.readerIndex(), buf.readableBytes())}.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     */
    @Override
    public ByteBuf copy() {
        return parent.copy();
    }

    /**
     * Returns a copy of this buffer's sub-region.  Modifying the content of
     * the returned buffer or this buffer does not affect each other at all.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     */
    @Override
    public ByteBuf copy(int index, int length) {
        return parent.copy(index, length);
    }

    /**
     * Returns a slice of this buffer's readable bytes. Modifying the content
     * of the returned buffer or this buffer affects each other's content
     * while they maintain separate indexes and marks.  This method is
     * identical to {@code buf.slice(buf.readerIndex(), buf.readableBytes())}.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     */
    @Override
    public ByteBuf slice() {
        return parent.slice();
    }

    /**
     * Returns a slice of this buffer's sub-region. Modifying the content of
     * the returned buffer or this buffer affects each other's content while
     * they maintain separate indexes and marks.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     */
    @Override
    public ByteBuf slice(int index, int length) {
        return parent.slice(index, length);
    }

    /**
     * Returns a buffer which shares the whole region of this buffer.
     * Modifying the content of the returned buffer or this buffer affects
     * each other's content while they maintain separate indexes and marks.
     * This method is identical to {@code buf.slice(0, buf.capacity())}.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     */
    @Override
    public ByteBuf duplicate() {
        return parent.duplicate();
    }

    /**
     * Returns the maximum number of NIO {@link ByteBuffer}s that consist this buffer.  Take in account that {@link #nioBuffers()}
     * or {@link #nioBuffers(int, int)} might return a less number of {@link ByteBuffer}s.
     *
     * @return {@code -1} if this buffer has no underlying {@link ByteBuffer}.
     * the number of the underlying {@link ByteBuffer}s if this buffer has at least one underlying
     * {@link ByteBuffer}.  Take in account that this method does not return {@code 0} to avoid confusion.
     * @see #nioBuffer()
     * @see #nioBuffer(int, int)
     * @see #nioBuffers()
     * @see #nioBuffers(int, int)
     */
    @Override
    public int nioBufferCount() {
        return parent.nioBufferCount();
    }

    /**
     * Exposes this buffer's readable bytes as an NIO {@link ByteBuffer}.  The returned buffer
     * shares the content with this buffer, while changing the position and limit of the returned
     * NIO buffer does not affect the indexes and marks of this buffer.  This method is identical
     * to {@code buf.nioBuffer(buf.readerIndex(), buf.readableBytes())}.  This method does not
     * modify {@code readerIndex} or {@code writerIndex} of this buffer.  Please take in account that the
     * returned NIO buffer will not see the changes of this buffer if this buffer is a dynamic
     * buffer and it adjusted its capacity.
     *
     * @throws UnsupportedOperationException if this buffer cannot create a {@link ByteBuffer} that shares the content with itself
     * @see #nioBufferCount()
     * @see #nioBuffers()
     * @see #nioBuffers(int, int)
     */
    @Override
    public ByteBuffer nioBuffer() {
        return parent.nioBuffer();
    }

    /**
     * Exposes this buffer's sub-region as an NIO {@link ByteBuffer}.  The returned buffer
     * shares the content with this buffer, while changing the position and limit of the returned
     * NIO buffer does not affect the indexes and marks of this buffer.  This method does not
     * modify {@code readerIndex} or {@code writerIndex} of this buffer.  Please take in account that the
     * returned NIO buffer will not see the changes of this buffer if this buffer is a dynamic
     * buffer and it adjusted its capacity.
     *
     * @throws UnsupportedOperationException if this buffer cannot create a {@link ByteBuffer} that shares the content with itself
     * @see #nioBufferCount()
     * @see #nioBuffers()
     * @see #nioBuffers(int, int)
     */
    @Override
    public ByteBuffer nioBuffer(int index, int length) {
        return parent.nioBuffer(index, length);
    }

    /**
     * Internal use only: Exposes the internal NIO buffer.
     *
     */
    @Override
    public ByteBuffer internalNioBuffer(int index, int length) {
        return parent.internalNioBuffer(index, length);
    }

    /**
     * Exposes this buffer's readable bytes as an NIO {@link ByteBuffer}'s.  The returned buffer
     * shares the content with this buffer, while changing the position and limit of the returned
     * NIO buffer does not affect the indexes and marks of this buffer. This method does not
     * modify {@code readerIndex} or {@code writerIndex} of this buffer.  Please take in account that the
     * returned NIO buffer will not see the changes of this buffer if this buffer is a dynamic
     * buffer and it adjusted its capacity.
     *
     * @throws UnsupportedOperationException if this buffer cannot create a {@link ByteBuffer} that shares the content with itself
     * @see #nioBufferCount()
     * @see #nioBuffer()
     * @see #nioBuffer(int, int)
     */
    @Override
    public ByteBuffer[] nioBuffers() {
        return parent.nioBuffers();
    }

    /**
     * Exposes this buffer's bytes as an NIO {@link ByteBuffer}'s for the specified index and length
     * The returned buffer shares the content with this buffer, while changing the position and limit
     * of the returned NIO buffer does not affect the indexes and marks of this buffer. This method does
     * not modify {@code readerIndex} or {@code writerIndex} of this buffer.  Please take in account that the
     * returned NIO buffer will not see the changes of this buffer if this buffer is a dynamic
     * buffer and it adjusted its capacity.
     *
     * @throws UnsupportedOperationException if this buffer cannot create a {@link ByteBuffer} that shares the content with itself
     * @see #nioBufferCount()
     * @see #nioBuffer()
     * @see #nioBuffer(int, int)
     */
    @Override
    public ByteBuffer[] nioBuffers(int index, int length) {
        return parent.nioBuffers(index, length);
    }

    /**
     * Returns {@code true} if and only if this buffer has a backing byte array.
     * If this method returns true, you can safely call {@link #array()} and
     * {@link #arrayOffset()}.
     */
    @Override
    public boolean hasArray() {
        return parent.hasArray();
    }

    /**
     * Returns the backing byte array of this buffer.
     *
     * @throws UnsupportedOperationException if there no accessible backing byte array
     */
    @Override
    public byte[] array() {
        return parent.array();
    }

    /**
     * Returns the offset of the first byte within the backing byte array of
     * this buffer.
     *
     * @throws UnsupportedOperationException if there no accessible backing byte array
     */
    @Override
    public int arrayOffset() {
        return parent.arrayOffset();
    }

    /**
     * Returns {@code true} if and only if this buffer has a reference to the low-level memory address that points
     * to the backing data.
     */
    @Override
    public boolean hasMemoryAddress() {
        return parent.hasMemoryAddress();
    }

    /**
     * Returns the low-level memory address that point to the first byte of ths backing data.
     *
     * @throws UnsupportedOperationException if this buffer does not support accessing the low-level memory address
     */
    @Override
    public long memoryAddress() {
        return parent.memoryAddress();
    }

    /**
     * Decodes this buffer's readable bytes into a string with the specified
     * character set name.  This method is identical to
     * {@code buf.toString(buf.readerIndex(), buf.readableBytes(), charsetName)}.
     * This method does not modify {@code readerIndex} or {@code writerIndex} of
     * this buffer.
     *
     * @throws UnsupportedCharsetException if the specified character set name is not supported by the
     * current VM
     */
    @Override
    public String toString(Charset charset) {
        return parent.toString(charset);
    }

    /**
     * Decodes this buffer's sub-region into a string with the specified
     * character set.  This method does not modify {@code readerIndex} or
     * {@code writerIndex} of this buffer.
     *
     */
    @Override
    public String toString(int index, int length, Charset charset) {
        return parent.toString(index, length, charset);
    }

    /**
     * Returns a hash code which was calculated from the content of this
     * buffer.  If there's a byte array which is
     * {@linkplain #equals(Object) equal to} this array, both arrays should
     * return the same value.
     */
    @Override
    public int hashCode() {
        return parent.hashCode();
    }

    /**
     * Determines if the content of the specified buffer is identical to the
     * content of this array.  'Identical' here means:
     * <ul>
     * <li>the size of the contents of the two buffers are same and</li>
     * <li>every single byte of the content of the two buffers are same.</li>
     * </ul>
     * Please take in account that it does not compare {@link #readerIndex()} nor
     * {@link #writerIndex()}.  This method also returns {@code false} for
     * {@code null} and an object which is not an instance of
     * {@link ByteBuf} type.
     *
     */
    @Override
    public boolean equals(Object obj) {
        return parent.equals(obj);
    }

    /**
     * Compares the content of the specified buffer to the content of this
     * buffer.  Comparison is performed in the same manner with the string
     * comparison functions of various languages such as {@code strcmp},
     * {@code memcmp} and {@link String#compareTo(String)}.
     *
     */
    @Override
    public int compareTo(ByteBuf buffer) {
        return parent.compareTo(buffer);
    }

    /**
     * Returns the string representation of this buffer.  This method does not
     * necessarily return the whole content of the buffer but returns
     * the values of the key properties such as {@link #readerIndex()},
     * {@link #writerIndex()} and {@link #capacity()}.
     */
    @Override
    public String toString() {
        return parent.toString();
    }

    @Override
    public ByteBuf retain(int increment) {
        return parent.retain(increment);
    }

    /**
     * Returns the reference count of this object.  If {@code 0}, it means this object has been deallocated.
     */
    @Override
    public int refCnt() {
        return parent.refCnt();
    }

    @Override
    public ByteBuf retain() {
        return parent.retain();
    }

    @Override
    public ByteBuf touch() {
        return parent.touch();
    }

    @Override
    public ByteBuf touch(Object hint) {
        return parent.touch(hint);
    }

    /**
     * Decreases the reference count by {@code 1} and deallocates this object if the reference count reaches at
     * {@code 0}.
     *
     * @return {@code true} if and only if the reference count became {@code 0} and this object has been deallocated
     */
    @Override
    public boolean release() {
        return parent.release();
    }

    /**
     * Decreases the reference count by the specified {@code decrement} and deallocates this object if the reference
     * count reaches at {@code 0}.
     *
     * @return {@code true} if and only if the reference count became {@code 0} and this object has been deallocated
     */
    @Override
    public boolean release(int decrement) {
        return parent.release(decrement);
    }
}