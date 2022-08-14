package heast.core.security;

import java.math.BigInteger;
import java.util.Map;

public final class Keychain {

    private final Map<String, BigInteger> keys;

    public Keychain(Map<String, BigInteger> keys) {
        this.keys = keys;
    }

    public BigInteger getPrivateKey() {
        return keys.get("private");
    }

    public BigInteger getPublicKey() {
        return keys.get("public");
    }

    public BigInteger getModulus() {
        return keys.get("modulus");
    }
}
