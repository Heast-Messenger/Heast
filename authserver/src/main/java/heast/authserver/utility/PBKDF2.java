package heast.authserver.utility;

import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.PBEKeySpec;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;
import java.security.spec.InvalidKeySpecException;
import java.util.Base64;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

/**
 * Password-Based Key Derivation Function 2 (PBKDF2) with HMAC-SHA1.
 * <a href="https://en.wikipedia.org/wiki/PBKDF2">Link to Wiki</a>
 */
public final class PBKDF2 {
    private static final String ID = "$31$";
    private static final String ALGO = "PBKDF2WithHmacSHA256";
    private static final int SIZE = 128;
    private static final Pattern LAYOUT = Pattern.compile("\\$31\\$(\\d\\d?)\\$(.{43})");
    private static final SecureRandom RANDOM = new SecureRandom();
    private static final int COST = 16;

    private static int iterations(int cost) {
        return 1 << cost;
    }

    /**
     * Hash a password for storage. Equivalent to php's password_hash.
     * @param password The password to hash.
     * @return a secure authentication token to be stored for later authentication
     */
    public static String hash(char[] password) {
        final byte[] salt = new byte[SIZE / 8];
        RANDOM.nextBytes(salt);
        final byte[] dk = pbkdf2(password, salt, 1 << COST);
        final byte[] hash = new byte[salt.length + dk.length];
        System.arraycopy(salt, 0, hash, 0, salt.length);
        System.arraycopy(dk, 0, hash, salt.length, dk.length);
        final Base64.Encoder encoder = Base64.getUrlEncoder().withoutPadding();
        return ID + COST + '$' + encoder.encodeToString(hash);
    }

    public static boolean verify(char[] password, String token) {
        Matcher matcher = LAYOUT.matcher(token);
        if (!matcher.matches()) {
            return false;
        }
        final int iterations = iterations(Integer.parseInt(matcher.group(1)));
        final byte[] hash = Base64.getUrlDecoder().decode(matcher.group(2));
        final byte[] salt = new byte[SIZE / 8];
        System.arraycopy(hash, 0, salt, 0, salt.length);
        final byte[] check = pbkdf2(password, salt, iterations);
        int zero = 0;
        for (int idx = 0; idx < check.length; idx++) {
            if (hash[idx + salt.length] != check[idx]) {
                zero |= hash[idx + salt.length] ^ check[idx];
            }
        }
        return zero == 0;
    }

    private static byte[] pbkdf2(char[] password, byte[] salt, int iterations) {
        try {
            return SecretKeyFactory.getInstance(ALGO)
                .generateSecret(
                    new PBEKeySpec(password, salt, iterations, SIZE)
                ).getEncoded();
        } catch (NoSuchAlgorithmException e) {
            System.err.println("No such algorithm: " + ALGO);
            return null;
        } catch (InvalidKeySpecException e) {
            System.err.println("Invalid key spec");
            return null;
        }
    }
}
