package heast.core.security;

import javax.crypto.Cipher;
import javax.crypto.spec.SecretKeySpec;
import java.nio.charset.StandardCharsets;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;
import java.util.Arrays;
import java.util.Base64;

public final class AES {

    public static final AES INSTANCE = new AES();

    /**
     * Generates a new symmetric AES key.
     * @return The symmetric key.
     */
    public byte[] genKey(){
        StringBuilder key= new StringBuilder();
        SecureRandom r= new SecureRandom();
        for (int i=0;i<64;i++) {
            int x = r.nextInt(128);
            key.append((char) x);
        }
        return key.toString().getBytes();
    }

    /**
     * Returns a secret KeySpec for the given key using SHA-1.
     * @param myKey The key.
     * @return The secret KeySpec.
     */
    private SecretKeySpec getKeySpec(final byte[] myKey) {
        try {
            byte[] key = myKey;
            key = MessageDigest.getInstance("SHA-1")
                .digest(key);
            key = Arrays.copyOf(
                key, 16
            );
            return new SecretKeySpec(key, "AES");
        } catch (NoSuchAlgorithmException e) {
            e.printStackTrace();
            return null;
        }
    }

    /**
     * Encrypts the given plaintext with the given key.
     * @param str The plaintext to encrypt.
     * @param secret The key.
     * @return The ciphertext.
     */
    public byte[] encrypt(final byte[] str, final byte[] secret) {
        try {
            SecretKeySpec secretKey = getKeySpec(secret);
            Cipher cipher = Cipher.getInstance("AES/ECB/PKCS5Padding");
            cipher.init(Cipher.ENCRYPT_MODE, secretKey);
            return Base64.getEncoder()
                .encodeToString(cipher.doFinal(str))
                .getBytes();
        } catch (Exception e) {
            System.out.println("Error while encrypting: " + e);
        }
        return null;
    }

    /**
     * Decrypts the given ciphertext with the given key.
     * @param str The ciphertext to decrypt.
     * @param secret The key.
     * @return The plaintext.
     */
    public byte[] decrypt(final byte[] str, final byte[] secret) {
        try {
            SecretKeySpec secretKey = getKeySpec(secret);
            Cipher cipher = Cipher.getInstance("AES/ECB/PKCS5PADDING");
            cipher.init(Cipher.DECRYPT_MODE, secretKey);
            return new String(cipher.doFinal(Base64.getDecoder()
                .decode(str)))
                .getBytes();
        } catch (Exception e) {
            System.out.println("Error while decrypting: " + e);
            return null;
        }
    }
}