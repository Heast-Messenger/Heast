package heast.core.security;

import heast.core.logging.IO;

import javax.crypto.Cipher;
import java.security.Key;
import java.security.KeyPair;
import java.security.KeyPairGenerator;
import java.security.NoSuchAlgorithmException;

public final class RSA {

    private static final String ALGORITHM = "RSA";
    private static final int KEY_LENGTH = 4096;

    public static KeyPair generateKeyPair() {
        try {
            var keyPairGenerator = KeyPairGenerator.getInstance(ALGORITHM);
            keyPairGenerator.initialize(KEY_LENGTH);
            return keyPairGenerator.generateKeyPair();
        } catch (NoSuchAlgorithmException e) {
            IO.error.println("Failed to generate RSA keypair: " + e.getMessage());
            throw new RuntimeException(e);
        }
    }

    private static byte[] crypt(Key key, byte[] data, int mode) throws RuntimeException {
        try {
            var cipher = Cipher.getInstance(ALGORITHM);
            cipher.init(mode, key);
            return cipher.doFinal(data);
        } catch (Exception e) {
            throw new RuntimeException("Failed to crypt data: " + e.getMessage());
        }
    }

    public static byte[] encrypt(Key key, byte[] data) {
        try {
            return crypt(key, data, Cipher.ENCRYPT_MODE);
        } catch (Exception e) {
            IO.error.println("Failed to encrypt data: " + e.getMessage());
            throw new RuntimeException(e);
        }
    }

    public static byte[] decrypt(Key key, byte[] data) {
        try {
            return crypt(key, data, Cipher.DECRYPT_MODE);
        } catch (Exception e) {
            IO.error.println("Failed to decrypt data: " + e.getMessage());
            throw new RuntimeException(e);
        }
    }
}
