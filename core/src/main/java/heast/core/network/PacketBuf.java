package heast.core.network;

import io.netty.buffer.ByteBuf;
import heast.core.security.AES;
import heast.core.security.RSA;
import heast.core.utility.ByteBufImpl;

import java.math.BigInteger;
import java.nio.charset.StandardCharsets;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

public class PacketBuf extends ByteBufImpl {

    public PacketBuf(ByteBuf buf) {
        super(buf);
    }

    /**
     * Encrypts this packet with the given symmetric AES key.
     * @param symmetricKey The symmetric key to encrypt with.
     */
    public void encrypt(final byte[] symmetricKey) {
        byte[] bytes = new byte[readableBytes()];
        getBytes(0, bytes);
        byte[] encrypted = AES.INSTANCE.encrypt(
            bytes, symmetricKey
        );
        if (encrypted != null) {
            setBytes(0, encrypted);
        } else {
            throw new RuntimeException("Encryption failed.");
        }
    }

    /**
     * Decrypts this packet with the given symmetric AES key.
     * @param symmetricKey The symmetric key to decrypt with.
     */
    public void decrypt(final byte[] symmetricKey) {
        byte[] bytes = new byte[readableBytes()];
        getBytes(0, bytes);
        byte[] decrypted = AES.INSTANCE.decrypt(
            bytes, symmetricKey
        );
        if (decrypted != null) {
            setBytes(0, decrypted);
        } else {
            throw new IllegalStateException("Decryption failed.");
        }
    }

    /**
     * Encrypts this packet with the given asymmetric public RSA key.
     * @param publicKey The public key to encrypt with.
     * @param modulus The modulus to encrypt with.
     */
    public void encrypt(final BigInteger publicKey, final BigInteger modulus) {
        byte[] bytes = new byte[readableBytes()];
        getBytes(0, bytes);
        System.out.println(new String(bytes));
        byte[] encrypted = RSA.INSTANCE.encrypt(
            bytes, publicKey, modulus
        );
        setBytes(0, encrypted);
    }

    /**
     * Decrypts this packet with the given asymmetric private RSA key.
     * @param privateKey The private key to decrypt with.
     * @param modulus The modulus to decrypt with.
     */
    public void decrypt(final BigInteger privateKey, final BigInteger modulus) {
        byte[] bytes = new byte[readableBytes()];
        getBytes(0, bytes);
        byte[] decrypted = RSA.INSTANCE.decrypt(
            bytes, privateKey, modulus
        );
        setBytes(0, decrypted);
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
     * Writes a by RSA encrypted string of any length to the buffer.
     * @param str The string to write.
     *
     * @see #readBytes() to read the String back (as raw Data).
     */
    public void writeBytesEncryptRSA(final String str, BigInteger e,BigInteger n){
        if (str != null) {
            final byte[] data= RSA.INSTANCE.encrypt(str.getBytes(),e,n);

            writeVarInt(data.length);
            writeBytes(data);
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
     * Reads an encrypted String of any length from the buffer.
     * @return The data read.
     *
     * @see #writeBytesEncryptRSA(String,BigInteger,BigInteger) to write the string to the buffer.
     */
    public byte[] readBytes() {
        int len = readVarInt();
        if (len != -1) {

            byte[] data= new byte[len];

            readBytes(data);

            return data;
        } else {
            return null;
        }
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
     * Writes an RSA Key to the buffer.
     * @param key The key to write.
     *
     * @see #readRSAKey() to read the key back.
     */
    public void writeRSAKey(final BigInteger key) {
        if (key != null) {
            writeString(key.toString());
        } else {
            writeString(null);
        }
    }

    /**
     * Reads an RSA Key from the buffer.
     * @return The key read.
     *
     * @see #writeRSAKey(BigInteger) to write the key to the buffer.
     */
    public final BigInteger readRSAKey() {
        String key = readString();
        if (!key.isEmpty()) {
            return new BigInteger(key);
        } else {
            return null;
        }
    }

    /**
     * Writes an AES Key to the buffer.
     * @param key The key to write.
     *
     * @see #readAESKey() to read the key back.
     */
    public void writeAESKey(final byte[] key) {
        if (key != null && key.length != 64) {
            writeBytes(key);
        } else {
            writeByte(0x00);
        }
    }

    /**
     * Reads an AES Key from the buffer.
     * @return The key read.
     *
     * @see #writeAESKey(byte[]) to write the key to the buffer.
     */
    public final byte[] readAESKey() {
        if (readByte() != 0x00) {
            byte[] key = new byte[64];
            readBytes(key);
            return key;
        } else {
            return null;
        }
    }

    /**
     * Writes the shared modulus to the buffer.
     * @param modulus The modulus to write.
     *
     * @see #readModulus() to read the modulus back.
     */
    public void writeModulus(final BigInteger modulus) {
        if (modulus != null) {
            writeString(modulus.toString());
        } else {
            writeString(null);
        }
    }

    /**
     * Reads the shared modulus from the buffer.
     * @return The modulus read.
     *
     * @see #writeModulus(BigInteger) to write the modulus to the buffer.
     */
    public final BigInteger readModulus() {
        String modulus = readString();
        if (!modulus.isEmpty()) {
            return new BigInteger(modulus);
        } else {
            return null;
        }
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
            writeTimestamp(user.getSince());
            writeRSAKey(user.getPublicKey());
            writeRSAKey(user.getPrivateKey());
            writeRSAKey(user.getModulus());
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
                id, readString(), readString(), readString(), readTimestamp(), readRSAKey(), readRSAKey(), readModulus()
            );
        }  else {
            return null;
        }
    }
}
