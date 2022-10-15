package heast.core.network;

import heast.core.model.UserAccount;
import io.netty.buffer.ByteBuf;
import heast.core.utility.ByteBufImpl;

import java.nio.charset.StandardCharsets;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

public class PacketBuf extends ByteBufImpl {

    public PacketBuf(ByteBuf buf) {
        super(buf);
    }

    /**
     * Writes a variable-length integer to the buffer that may use less bytes than a regular integer.
     * @param value The integer to write.
     *
     * @see #readVarInt() to read the integer back.
     */
    public void writeVarInt(int value) {
        while ((value & -128) != 0) {
            writeByte(value & 127 | 128);
            value >>>= 7;   //what tha seven doin?
        }
        writeByte(value);
    }

    /**
     * Reads a variable-length integer from the buffer.
     * @return The integer that was read.
     */
    public int readVarInt() {
        int run = 0, len = 0;
        byte cur;
        do {
            cur = readByte();
            run |= (cur & 127) << len++ * 7;
            if (len > 5) {
                throw new RuntimeException("VarInt too big");
            }
        } while ((cur & 128) == 128);

        return run;
    }

    /**
     * Writes a string of any length to the buffer.
     * @param str The string to write.
     *
     * @see #readString() to read the string back.
     */
    public void writeString(final String str) {
        if (str != null) {
            writeVarInt(str.length());
            writeBytes(str.getBytes());
        } else {
            writeVarInt(-1);
        }
    }

    /**
     * Reads a string of any length from the buffer.
     * @return The string read.
     *
     * @see #writeString(String) to write the string to the buffer.
     */
    public String readString() {
        int len = readVarInt();
        if (len != -1) {
            return readBytes(len).toString(StandardCharsets.UTF_8);
        } else {
            return "";
        }
    }

    /**
     * Writes a byte array of any length to the buffer.
     * @param bytes The byte array to write.
     */
    public void writeByteArray(final byte[] bytes) {
        writeVarInt(bytes.length);
        writeBytes(bytes);
    }

    /**
     * Reads a byte array of any length from the buffer.
     * @return The byte array read.
     */
    public byte[] readByteArray() {
        var bytes = new byte[readVarInt()];
        readBytes(bytes);
        return bytes;
    }

    /**
     * Writes an enum to the buffer.
     * @param value The enum to write.
     * @param <E> The enum type.
     */
    public <E extends Enum<E>> void writeEnum(final E value) {
        writeVarInt(value.ordinal());
    }

    /**
     * Reads an enum from the buffer.
     * @param <E> The enum type.
     * @param clazz The enum class.
     * @return The enum read.
     */
    public <E extends Enum<E>> E readEnum(final Class<E> clazz) {
        return clazz.getEnumConstants()[readVarInt()];
    }

    /**
     * Writes a timestamp to the buffer.
     * @param timestamp The timestamp to write.
     *
     * @see #readTimestamp() to read the timestamp back.
     */
    public void writeTimestamp(final LocalDateTime timestamp) {
        writeString(timestamp.format(
            DateTimeFormatter.ofPattern("yyyyMMddHHmmss")
        ));
    }

    /**
     * Reads a timestamp from the buffer.
     * @return The timestamp read.
     *
     * @see #writeTimestamp(LocalDateTime) to write the timestamp to the buffer.
     */
    public final LocalDateTime readTimestamp() {
        return LocalDateTime.parse(readString(),
            DateTimeFormatter.ofPattern("yyyyMMddHHmmss")
        );
    }

    /**
     * Writes a user to the buffer.
     * @param user The user to write.
     */
    public void writeUser(final UserAccount user) {
        if (user != null) {
            writeVarInt(user.getId());
            writeString(user.getUsername());
            writeString(user.getEmail());
            writeString(user.getPassword());
        } else {
            writeVarInt(-1);
        }
    }

    /**
     * Reads a user from the buffer.
     * @return The user read.
     */
    public final UserAccount readUser() {
        int id = readVarInt();
        if (id != -1) {
            return new UserAccount(
                id, readString(), readString(), readString()
            );
        }  else {
            return null;
        }
    }
}
