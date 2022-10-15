package heast.client.network;

import heast.client.model.Settings;
import heast.core.logging.IO;
import heast.core.model.ClientInfo;
import heast.core.network.packets.c2s.auth.*;
import heast.core.network.packets.c2s.login.HelloC2SPacket;
import heast.core.network.pipeline.ClientConnection;
import heast.core.utility.Validator;
import javafx.application.Platform;

public final class ClientNetwork {
    
    public static ClientInfo clientInfo;
    
    public static ClientConnection authConnection;
    public static ClientConnection chatConnection;

    public static void initialize(String host, int port) {
        IO.info.println("Initializing client network...");
        clientInfo = new ClientInfo("TestOS");
        new Thread("Authentication Server Connection") {
            public void run() {
                authConnection = ClientConnection.connect(host, port);
                authConnection.setListener(new ClientLoginHandler(authConnection));
                authConnection.send(new HelloC2SPacket(clientInfo), () ->
                    IO.info.println("Sent client info to authentication server.")
                );
            }
        }.start();
    }

    public static void shutdown() {
        IO.info.println("Shutting down client network...");
    }

    public static void signup(String username, String email, String password, Runnable onSent) {
        if (!Validator.isUsernameValid(username)) {
            IO.error.println("Invalid username");
            return;
        }

        if (!Validator.isEmailValid(email)) {
            IO.error.println("Invalid email");
            return;
        }

        if (!Validator.isPasswordValid(password)) {
            IO.error.println("Invalid password");
            return;
        }

        authConnection.send(new SignupC2SPacket(
            username, email, password
        ), () -> {
            Platform.runLater(onSent);
            IO.info.format("Signing up as %s; %s; %s%n", username, email, password);
        });
    }

    public static void deleteAccount(String email, Runnable onSent){
        if (!Validator.isEmailValid(email)) {
            IO.error.println("Invalid email");
            return;
        }

        authConnection.send(new DeleteC2SPacket(email), () -> {
            Platform.runLater(onSent);
            IO.info.format("Deleting account %s%n", email);
        });
    }

    public static void login(String email, String password, Runnable onSent) {
        if (!Validator.isEmailValid(email)) {
            IO.error.println("Invalid email");
            return;
        }

        if (!Validator.isPasswordValid(password)) {
            IO.error.println("Invalid password");
            return;
        }

        authConnection.send(new LoginC2SPacket(
            email, password
        ), () -> {
            Platform.runLater(onSent);
            IO.info.format("Logging in as %s; %s%n", email, password);
        });
    }

    public static void reset(String email, String password, Runnable onSent) {
        if (!Validator.isEmailValid(email)) {
            IO.error.println("Invalid email");
            return;
        }

        if (!Validator.isPasswordValid(password)) {
            IO.error.println("Invalid password");
            return;
        }

        authConnection.send(new ResetC2SPacket(
            email, password
        ), () -> {
            Platform.runLater(onSent);
            IO.info.format("Resetting password for %s: %s%n", email, password);
        });
    }

    public static void logout() {
        authConnection.send(new LogoutC2SPacket(), () -> {
            IO.info.println("Logging out...");
        });
    }

    public static void verify(String code) {
        if (!Validator.isVerificationCodeValid(code)) {
            IO.error.println("Invalid verification code");
            return;
        }

        authConnection.send(new VerifyC2SPacket(code), () -> {
            IO.info.format("Verifying account with code: %s%n", code);
        });
    }

    public static void resend(String email) {
        if (!Validator.isEmailValid(email)) {
            IO.error.println("Invalid email");
            return;
        }
    }

    public static void cancel() {

    }

    public static boolean testConnection(String host, int port) {
        if (!Validator.isIpAddressValid(host) || !Validator.isPortValid(port)) {
            IO.error.println("Invalid server address");
            return false;
        }

        return true;
    }

    public static void tryAddServer(String host, int port) {
        if (!Validator.isIpAddressValid(host) || !Validator.isPortValid(port)) {
            IO.error.println("Invalid server address");
            return;
        }
    }
}
