package heast.client.control.network;

import io.netty.channel.Channel;
import heast.core.network.ClientConnection;
import heast.core.network.c2s.*;
import heast.core.utility.Validator;

import java.math.BigInteger;

public final class ClientNetwork {

    public static final ClientNetwork INSTANCE = new ClientNetwork();

    public ClientConnection connection;
    public byte[] symmetricKey;

    public BigInteger serverPublicKey;
    public BigInteger serverModulus;

    public void initialize() {
        System.out.println("Initializing client network...");
    }

    public void shutdown() {
        System.out.println("Shutting down client network...");
        if (this.connection != null) {
            connection.send(
                new LogoutC2SPacket(LogoutC2SPacket.Reason.CLIENT_QUIT)
            );
            Channel channel = connection.getChannel();
            if (channel != null) {
                channel.close();
            }
        }
    }

    public void signup(String uname, String email, String password) {
        System.out.println("Signing up...");
        String username = uname.replace(" ", "_").trim();
        String address = email.trim();

        if (!Validator.isUsernameValid(username)) {
            System.err.println("Invalid username");
            return;
        }

        if (!Validator.isEmailValid(address)) {
            System.err.println("Invalid email");
            return;
        }

        if (!Validator.isPasswordValid(password)) {
            System.err.println("Invalid password");
            return;
        }

        connection.send(
            new SignupC2SPacket(
                username, address, password, serverPublicKey, serverModulus
            )
        );
    }

    public void deleteAccount(String email){
        if (!Validator.isEmailValid(email.trim())) {
            System.err.println("Invalid email");
            return;
        }
        connection.send(
                new DeleteAcC2SPacket(
                        email, serverPublicKey, serverModulus
                )
        );
    }

    public void login(String email, String password) {
        System.out.println("Logging in...");
        String address = email.trim();

        if (!Validator.isEmailValid(address)) {
            System.err.println("Invalid email");
            return;
        }

        if (!Validator.isPasswordValid(password)) {
            System.err.println("Invalid password");
            return;
        }

        connection.send(
            new LoginC2SPacket(
                address, password, serverPublicKey, serverModulus
            )
        );
    }

    public void reset(String email, String newPassword) {
        System.out.println("Resetting Account...");
        String address = email.trim();

        if (!Validator.isEmailValid(address)) {
            System.err.println("Invalid email");
            return;
        }

        if (!Validator.isPasswordValid(newPassword)) {
            System.err.println("Invalid password");
            return;
        }

        connection.send(
            new ResetC2SPacket(address, newPassword, serverPublicKey, serverModulus)
        );
    }

    public void logout() {
        System.out.println("Logging out...");
        connection.send(new LogoutC2SPacket(LogoutC2SPacket.Reason.LOGOUT));
    }

    public void verify(String verificationCode) {
        if (!Validator.isVerificationCodeValid(verificationCode)) {
            System.err.println("Invalid verification code");
            return;
        }

        System.out.println("Verifying Account with code: " + verificationCode + "...");

        connection.send(
            new VerificationC2SPacket(verificationCode, serverPublicKey, serverModulus)
        );
    }

    public void tryAddServer(String host, int port) {
        if (!Validator.isIpAddressValid(host) || !Validator.isPortValid(port)) {
            System.err.println("Invalid server address");
            return;
        }
    }

    public void testConnection(String host, int port) {
        if (!Validator.isIpAddressValid(host) || !Validator.isPortValid(port)) {
            System.err.println("Invalid server address");
            return;
        }
    }
}
