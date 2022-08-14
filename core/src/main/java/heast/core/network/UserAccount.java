package heast.core.network;

import java.math.BigInteger;
import java.time.LocalDateTime;

public final class UserAccount {
    public final int id;
    public final String username;
    public final String email;
    public final String password;
    public final LocalDateTime since;
    public final BigInteger publicKey;
    public final BigInteger privateKey;
    public final BigInteger modulus;
    public final String avatar;

    public UserAccount(int id, String username, String email, String password, LocalDateTime since, BigInteger publicKey, BigInteger privateKey, BigInteger modulus) {
        this.id = id;
        this.username = username;
        this.email = email;
        this.password = password;
        this.since = since;
        this.publicKey = publicKey;
        this.privateKey = privateKey;
        this.modulus = modulus;
        this.avatar = "http://localhost:8000/avatars?id=" + id;
    }

    public int getId() {
        return id;
    }

    public String getUsername() {
        return username;
    }

    public String getEmail() {
        return email;
    }

    public String getPassword() {
        return password;
    }

    public LocalDateTime getSince() {
        return since;
    }

    public BigInteger getPublicKey() {
        return publicKey;
    }

    public BigInteger getPrivateKey() {
        return privateKey;
    }

    public BigInteger getModulus() {
        return modulus;
    }

    public String getAvatar() {
        return avatar;
    }

    @Override
    public String toString() {
        return "UserAccount{" + "id=" + id + ", username='" + username + '\'' + ", email='" + email + '\'' + ", password='" + password + '\'' + ", since=" + since + ", publicKey=" + publicKey + ", privateKey=" + privateKey + ", modulus=" + modulus + ", avatar='" + avatar + '\'' + '}';
    }
}