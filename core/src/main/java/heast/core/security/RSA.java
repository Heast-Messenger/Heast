package heast.core.security;

import java.math.BigInteger;
import java.security.SecureRandom;
import java.util.HashMap;
import java.util.Map;

/**
 * p = Primzahl 1; Länge: 617 characters
 * q = Primzahl 2; Länge: 2466 characters
 * N = Teil beider Keys; Länge: 2466 characters
 *
 * phi = Eulersche phi-funktion
 * e = Teil des public key
 * d = Teil des private key
 */
public final class RSA {

    public static final RSA INSTANCE = new RSA();

    /**
     * Generate a new RSA keypair. The keypair consists of a private and public key + a shared modulus N.
     * @return A new RSA keypair.
     */
    public Keychain genKeyPair() {
        // NOTE: Der braucht manchmal über 30 sekunden zu generieren xD
        //  Kann man den irgendwie schneller machen?
        SecureRandom r= new SecureRandom();
        int bitLength = 4096;
        BigInteger p= BigInteger.probablePrime(bitLength,r);
        BigInteger q= BigInteger.probablePrime(bitLength,r);
        // Bis dahin ----------------------------------------
        BigInteger N= p.multiply(q);
        //Da p und q primzahlen sind, muss man nur 1 wegrechnen (Mathe henker)
        BigInteger phi= p.subtract(BigInteger.ONE).multiply(q.subtract(BigInteger.ONE));
        BigInteger e= BigInteger.probablePrime(bitLength /2,r);
        while (phi.gcd(e).compareTo(BigInteger.ONE) > 0 && e.compareTo(phi) < 0) {
            e= e.add(BigInteger.ONE);
        }
        BigInteger d= e.modInverse(phi);

        final HashMap<String, BigInteger> keys = new HashMap<>();
        keys.put("private", d);
        keys.put("public", e);
        keys.put("modulus", N);

        return new Keychain(
            keys
        );
    }

    /**
     * Encrypts the given plaintext with the public key.
     * @param text The plaintext to encrypt.
     * @param key The public key.
     * @param N The modulus.
     * @return The ciphertext.
     */
    public byte[] encrypt(byte[] text, BigInteger key, BigInteger N) {
        return new BigInteger(text)
            .modPow(key,N)
            .toByteArray();
    }

    /**
     * Decrypts the given ciphertext with the private key.
     * @param cipher The ciphertext to decrypt.
     * @param key The private key.
     * @param N The modulus.
     * @return The plaintext.
     */
    public byte[] decrypt(byte[] cipher, BigInteger key, BigInteger N) {
        return new BigInteger(cipher)
            .modPow(key,N)
            .toByteArray();
    }
}
