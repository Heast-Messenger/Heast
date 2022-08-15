package heast.client.control.network;

import heast.client.view.dialog.Dialog;
import javafx.application.Platform;
import heast.client.model.Settings;
import heast.client.view.WelcomeView;
import heast.core.network.ClientConnection;
import heast.core.network.s2c.*;
import heast.core.network.s2c.listener.ClientAuthListener;

public final class ClientAuthHandler implements ClientAuthListener {

    private final ClientConnection connection;

    public ClientAuthHandler(ClientConnection connection) {
        this.connection = connection;
    }

    @Override
    public void onLoginResponse(LoginResponseS2CPacket buf) {
        switch (buf.getStatus()) {
            case OK -> {
                System.out.println("Server: Logged in!");
                Platform.runLater(() -> {
                    Settings.INSTANCE.getAccount().set(buf.getUser());
                });
            }

            case CODE_SENT -> {
                System.out.println("Server: Code sent!");
                Platform.runLater(() -> Dialog.INSTANCE.show(
                    WelcomeView.VerificationPane.INSTANCE, WelcomeView.INSTANCE
                ));
            }

            case INVALID_CREDENTIALS -> {
                System.out.println("Server: Invalid credentials");
            }

            case USER_NOT_FOUND -> {
                System.out.println("Server: User not found");
            }
        }
    }

    @Override
    public void onSignupResponse(SignupResponseS2CPacket buf) {
        switch (buf.getStatus()) {
            case OK -> {
                System.out.println("Server: Signed up!");
                Platform.runLater(() -> WelcomeView.INSTANCE.setPane(
                    WelcomeView.LoginPane.INSTANCE
                ));
            }

            case CODE_SENT -> {
                System.out.println("Server: Code sent!");
                Platform.runLater(() -> Dialog.INSTANCE.show(
                    WelcomeView.VerificationPane.INSTANCE, WelcomeView.INSTANCE
                ));
            }

            case INVALID_CREDENTIALS -> {
                System.out.println("Server: Invalid credentials");
            }

            case USER_EXISTS -> {
                System.out.println("Server: User already exists");
            }
        }
    }

    @Override
    public void onResetResponse(ResetResponseS2CPacket buf) {
        switch (buf.getStatus()) {
            case OK -> {
                System.out.println("Server: Password reset!");
                Platform.runLater(() -> WelcomeView.INSTANCE.setPane(
                    WelcomeView.LoginPane.INSTANCE
                ));
            }

            case CODE_SENT -> {
                System.out.println("Server: Code sent!");
                Platform.runLater(() -> Dialog.INSTANCE.show(
                    WelcomeView.VerificationPane.INSTANCE, WelcomeView.INSTANCE
                ));
            }

            case INVALID_CREDENTIALS -> {
                System.out.println("Server: Invalid credentials");
            }

            case USER_NOT_FOUND -> {
                System.out.println("Server: User not found");
            }
        }
    }

    @Override
    public void onServerPublicKeyResponse(ServerKeyResponseS2CPacket buf) {
        System.out.println("Server: Public key + Modulus received!");
        ClientNetwork.INSTANCE.serverPublicKey = buf.getPublicKey();
        ClientNetwork.INSTANCE.serverModulus = buf.getModulus();
    }
}