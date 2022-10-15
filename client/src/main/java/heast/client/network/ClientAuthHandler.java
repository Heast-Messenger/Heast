package heast.client.network;

import heast.client.gui.WelcomeView;
import heast.client.gui.dialog.Dialog;
import heast.client.gui.template.LoadingPane;
import heast.client.gui.welcome.LoginPane;
import heast.client.gui.welcome.VerificationPane;
import heast.client.model.Settings;
import heast.core.logging.IO;
import heast.core.network.listeners.ClientAuthListener;
import heast.core.network.packets.s2c.auth.*;
import heast.core.network.pipeline.ClientConnection;
import javafx.application.Platform;

public class ClientAuthHandler implements ClientAuthListener {

    private final ClientConnection connection;

    public ClientAuthHandler(ClientConnection connection) {
        this.connection = connection;
    }

    /**
     * Called when the server responds after a signup request.
     */
    @Override
    public void onSignupResponse(SignupResponseS2CPacket packet) {
        switch (packet.getStatus()) {
            case SUCCESS -> {
                IO.info.println("Signed up");
                Platform.runLater(() -> WelcomeView.INSTANCE.setPane(
                    LoginPane.INSTANCE
                ));
            }
            case AWAIT_VERIFICATION -> {
                IO.info.println("Awaiting verification");
                Platform.runLater(() -> {
                    Dialog.INSTANCE.show(
                        VerificationPane.INSTANCE, WelcomeView.INSTANCE);
                    Dialog.INSTANCE.close(
                        LoadingPane.Companion.getVerificationLoader(), WelcomeView.INSTANCE);
                });
            }
            case INVALID_CREDENTIALS -> {
                IO.error.println("Invalid credentials");
                Platform.runLater(() -> Dialog.INSTANCE.close(
                    LoadingPane.Companion.getVerificationLoader(), WelcomeView.INSTANCE
                ));
            }
            case USER_EXISTS -> {
                IO.error.println("User already exists");
                Platform.runLater(() -> Dialog.INSTANCE.close(
                    LoadingPane.Companion.getVerificationLoader(), WelcomeView.INSTANCE
                ));
            }
            case ERROR -> {
                IO.error.println("Unexpected error");
                Platform.runLater(() -> Dialog.INSTANCE.close(
                    LoadingPane.Companion.getVerificationLoader(), WelcomeView.INSTANCE
                ));
            }
        }
    }

    /**
     * Called when the server responds after a login request.
     */
    @Override
    public void onLoginResponse(LoginResponseS2CPacket packet) {
        switch (packet.getStatus()) {
            case SUCCESS -> {
                IO.info.println("Logged in");
                Platform.runLater(() -> {
                    Dialog.INSTANCE.close(
                        VerificationPane.INSTANCE, WelcomeView.INSTANCE);
                    Settings.INSTANCE.getAccount().set(packet.getAccount());
                });
            }
            case AWAIT_VERIFICATION -> {
                IO.info.println("Awaiting verification");
                Platform.runLater(() -> {
                    Dialog.INSTANCE.show(
                        VerificationPane.INSTANCE, WelcomeView.INSTANCE);
                    Dialog.INSTANCE.close(
                        LoadingPane.Companion.getVerificationLoader(), WelcomeView.INSTANCE);
                });
            }
            case INVALID_CREDENTIALS -> {
                IO.error.println("Invalid credentials");
                Platform.runLater(() -> Dialog.INSTANCE.close(
                    LoadingPane.Companion.getVerificationLoader(), WelcomeView.INSTANCE
                ));
            }
            case USER_NOT_FOUND -> {
                IO.error.println("User does not exists");
                Platform.runLater(() -> Dialog.INSTANCE.close(
                    LoadingPane.Companion.getVerificationLoader(), WelcomeView.INSTANCE
                ));
            }
            case ERROR -> {
                IO.error.println("Unexpected error");
                Platform.runLater(() -> Dialog.INSTANCE.close(
                    LoadingPane.Companion.getVerificationLoader(), WelcomeView.INSTANCE
                ));
            }
        }
    }

    /**
     * Called when the server responds after a password reset request.
     */
    @Override
    public void onResetResponse(ResetResponseS2CPacket packet) {
        switch (packet.getStatus()) {
            case SUCCESS -> {
                IO.info.println("Reset password");
                Platform.runLater(() -> WelcomeView.INSTANCE.setPane(
                    LoginPane.INSTANCE
                ));
            }
            case AWAIT_VERIFICATION -> {
                IO.info.println("Awaiting verification");
                Platform.runLater(() -> {
                    Dialog.INSTANCE.show(
                        VerificationPane.INSTANCE, WelcomeView.INSTANCE);
                    Dialog.INSTANCE.close(
                        LoadingPane.Companion.getVerificationLoader(), WelcomeView.INSTANCE);
                });
            }
            case INVALID_CREDENTIALS -> {
                IO.error.println("Invalid credentials");
                Platform.runLater(() -> Dialog.INSTANCE.close(
                    LoadingPane.Companion.getVerificationLoader(), WelcomeView.INSTANCE
                ));
            }
            case USER_NOT_FOUND -> {
                IO.error.println("User does not exists");
                Platform.runLater(() -> Dialog.INSTANCE.close(
                    LoadingPane.Companion.getVerificationLoader(), WelcomeView.INSTANCE
                ));
            }
            case ERROR -> {
                IO.error.println("Unexpected error");
                Platform.runLater(() -> Dialog.INSTANCE.close(
                    LoadingPane.Companion.getVerificationLoader(), WelcomeView.INSTANCE
                ));
            }
        }
    }

    /**
     * Called when the server responds after an account deletion request.
     */
    @Override
    public void onDeleteResponse(DeleteResponseS2CPacket packet) {
        switch (packet.getStatus()) {
            case SUCCESS -> {
                IO.info.println("Deleted account");
                Platform.runLater(() -> {
                    Settings.INSTANCE.getAccount().set(null);
                    WelcomeView.INSTANCE.setPane(
                        LoginPane.INSTANCE
                    );
                });
            }
            case AWAIT_VERIFICATION -> {
                IO.info.println("Awaiting verification");
                Platform.runLater(() -> {
                    Dialog.INSTANCE.show(
                        VerificationPane.INSTANCE, WelcomeView.INSTANCE);
                    Dialog.INSTANCE.close(
                        LoadingPane.Companion.getVerificationLoader(), WelcomeView.INSTANCE);
                });
            }
            case INVALID_EMAIL -> {
                IO.error.println("Invalid email");
                Platform.runLater(() -> Dialog.INSTANCE.close(
                    LoadingPane.Companion.getVerificationLoader(), WelcomeView.INSTANCE
                ));
            }
            case ERROR -> {
                IO.error.println("Unexpected error");
                Platform.runLater(() -> Dialog.INSTANCE.close(
                    LoadingPane.Companion.getVerificationLoader(), WelcomeView.INSTANCE
                ));
            }
        }
    }

    /**
     * Called when the server responds after a logout request.
     */
    @Override
    public void onLogoutResponse(LogoutResponseS2CPacket packet) {
        switch (packet.getStatus()) {
            case SUCCESS -> {
                IO.info.println("Logged out");
                Platform.runLater(() -> {
                    Settings.INSTANCE.getAccount().set(null);
                });
            }
            case ERROR -> {
                IO.error.println("Unexpected error");
            }
        }
    }

    /**
     * Called when the client fails to verify themselves
     */
    @Override
    public void onVerifyFailed() {
        IO.error.println("Wrong verification code, try again");
    }
}
