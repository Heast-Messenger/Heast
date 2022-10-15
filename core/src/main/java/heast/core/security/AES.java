package heast.core.security;

import heast.core.logging.IO;

import javax.crypto.Cipher;
import javax.crypto.KeyGenerator;
import javax.crypto.SecretKey;
import javax.crypto.spec.IvParameterSpec;
import java.security.Key;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;

public final class AES {

    private static final String ALGORITHM = "AES/CFB8/NoPadding";
    private static final int KEY_LENGTH = 128;

    public static SecretKey generateKey() {
        try {
            var keygen = KeyGenerator.getInstance("AES");
            keygen.init(KEY_LENGTH, new SecureRandom());
            return keygen.generateKey();
        } catch (NoSuchAlgorithmException e) {
            IO.error.println("Failed to generate AES key: " + e.getMessage());
            throw new RuntimeException(e);
        }
    }

    public static Cipher cipherFromKey(SecretKey key, int mode) {
        try {
            var cipher = Cipher.getInstance(ALGORITHM);
            cipher.init(mode, key, new IvParameterSpec(key.getEncoded()));
            return cipher;
        } catch (Exception e) {
            IO.error.println("Failed to create AES  cipher: " + e.getMessage());
            throw new RuntimeException(e);
        }
    }

    private static byte[] crypt(SecretKey key, byte[] data, int mode) throws RuntimeException {
        try {
            return cipherFromKey(key, mode).doFinal(data);
        } catch (Exception e) {
            throw new RuntimeException("Failed to crypt data: " + e.getMessage());
        }
    }

    public static byte[] encrypt(SecretKey key, byte[] data) {
        try {
            return crypt(key, data, Cipher.ENCRYPT_MODE);
        } catch (Exception e) {
            IO.error.println("Failed to encrypt data: " + e.getMessage());
            throw new RuntimeException(e);
        }
    }

    public static byte[] decrypt(SecretKey key, byte[] data) {
        try {
            return crypt(key, data, Cipher.DECRYPT_MODE);
        } catch (Exception e) {
            IO.error.println("Failed to decrypt data: " + e.getMessage());
            throw new RuntimeException(e);
        }
    }
}