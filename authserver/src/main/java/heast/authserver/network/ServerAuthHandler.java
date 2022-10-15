package heast.authserver.network;

import heast.authserver.components.Database;
import heast.core.logging.IO;
import heast.core.network.listeners.ServerAuthListener;
import heast.core.network.packets.c2s.auth.*;
import heast.core.network.packets.s2c.auth.*;
import heast.core.network.pipeline.ClientConnection;
import heast.core.utility.Validator;

public final class ServerAuthHandler implements ServerAuthListener {

    private final ClientConnection connection;

    public ServerAuthHandler(ClientConnection connection) {
        this.connection = connection;
    }

    /**
     * Called when the client registers a new account on the platform
     */
    @Override
    public void onSignup(SignupC2SPacket packet) {
        var username = packet.getUsername();
        var email = packet.getEmail();
        var password = packet.getPassword();

        if (!Validator.isUsernameValid(username) || !Validator.isEmailValid(email) || !Validator.isPasswordValid(password)) {
            connection.send(new SignupResponseS2CPacket(
                SignupResponseS2CPacket.Status.INVALID_CREDENTIALS
            ), () -> {
                IO.info.format("Client tried to sign up with invalid credentials: %s; %s; %s%n", username, email, password);
            });
            return;
        }

        if (Database.userExists(email)) {
            connection.send(new SignupResponseS2CPacket(
                SignupResponseS2CPacket.Status.USER_EXISTS
            ), () -> {
                IO.info.format("Client tried to sign up with %s, which is already registered.%n", email);
            });
            return;
        }

        ServerNetwork.sendVerificationCode(connection, email, () -> {
            connection.send(new SignupResponseS2CPacket(
                SignupResponseS2CPacket.Status.AWAIT_VERIFICATION
            ));
        }, () -> {
            boolean successful = ServerNetwork.registerClient(username, email, password);
            if (successful) {
                connection.send(new SignupResponseS2CPacket(
                    SignupResponseS2CPacket.Status.SUCCESS
                ));
            } else {
                connection.send(new SignupResponseS2CPacket(
                    SignupResponseS2CPacket.Status.ERROR
                ));
            }
        });
    }

    /**
     * Called when the client logs into their account
     */
    @Override
    public void onLogin(LoginC2SPacket packet) {
        var email = packet.getEmail();
        var password = packet.getPassword();

        if (!Validator.isEmailValid(email) || !Validator.isPasswordValid(password) || Database.userExists(email, password)) {
            connection.send(new LoginResponseS2CPacket(
                LoginResponseS2CPacket.Status.INVALID_CREDENTIALS, null
            ), () -> {
                IO.info.format("Client tried to login with invalid credentials: %s; %s%n", email, password);
            });
            return;
        }

        if (!Database.userExists(email)) {
            connection.send(new LoginResponseS2CPacket(
                LoginResponseS2CPacket.Status.USER_NOT_FOUND, null
            ), () -> {
                IO.info.format("Client tried to login with %s, which is not registered%n", email);
            });
            return;
        }

        ServerNetwork.sendVerificationCode(connection, email, () -> {
            connection.send(new LoginResponseS2CPacket(
                LoginResponseS2CPacket.Status.AWAIT_VERIFICATION, null
            ));
        }, () -> {
            var account = ServerNetwork.loginClient(email, password);
            if (account != null) {
                ServerNetwork.addClient(connection);
                connection.send(new LoginResponseS2CPacket(
                    LoginResponseS2CPacket.Status.SUCCESS, account
                ));
            } else {
                connection.send(new LoginResponseS2CPacket(
                    LoginResponseS2CPacket.Status.ERROR, null
                ));
            }
        });
    }

    /**
     * Called when the client resets their account password
     * Note that the password parameter is the new account password
     */
    @Override
    public void onReset(ResetC2SPacket packet) {
        var email = packet.getEmail();
        var password = packet.getPassword();

        if (!Validator.isEmailValid(email) || !Validator.isPasswordValid(password)) {
            connection.send(new ResetResponseS2CPacket(
                ResetResponseS2CPacket.Status.INVALID_CREDENTIALS
            ), () -> {
                IO.info.format("Client tried to reset their password with invalid credentials: %s; %s%n", email, password);
            });
            return;
        }

        if (!Database.userExists(email)) {
            connection.send(new ResetResponseS2CPacket(
                ResetResponseS2CPacket.Status.USER_NOT_FOUND
            ), () -> {
                IO.info.format("Client tried to reset their password with %s, which is not registered%n", email);
            });
            return;
        }

        ServerNetwork.sendVerificationCode(connection, email, () -> {
            connection.send(new ResetResponseS2CPacket(
                ResetResponseS2CPacket.Status.AWAIT_VERIFICATION
            ));
        }, () -> {
            boolean successful = ServerNetwork.resetClientPassword(email, password);
            if (successful) {
                connection.send(new ResetResponseS2CPacket(
                    ResetResponseS2CPacket.Status.SUCCESS
                ));
            } else {
                connection.send(new ResetResponseS2CPacket(
                    ResetResponseS2CPacket.Status.ERROR
                ));
            }
        });
    }

    /**
     * Called when the client deletes their account
     */
    @Override
    public void onDelete(DeleteC2SPacket packet) {
        var email = packet.getEmail();

        if (!Validator.isEmailValid(email)) {
            connection.send(new DeleteResponseS2CPacket(
                DeleteResponseS2CPacket.Status.INVALID_EMAIL
            ), () -> {
                IO.info.format("Client tried to delete their account with invalid credentials: %s%n", email);
            });
            return;
        }

        ServerNetwork.sendVerificationCode(connection, email, () -> {
            connection.send(new DeleteResponseS2CPacket(
                DeleteResponseS2CPacket.Status.AWAIT_VERIFICATION
            ));
        }, () -> {
            boolean successful = ServerNetwork.deleteClient(email);
            if (successful) {
                connection.send(new DeleteResponseS2CPacket(
                    DeleteResponseS2CPacket.Status.SUCCESS
                ));
            } else {
                connection.send(new DeleteResponseS2CPacket(
                    DeleteResponseS2CPacket.Status.ERROR
                ));
            }
        });
    }

    /**
     * Called when the client logs out of their account
     */
    @Override
    public void onLogout() {
        boolean successful = ServerNetwork.logoutClient(connection);
        if (successful) {
            connection.send(new LogoutResponseS2CPacket(
                LogoutResponseS2CPacket.Status.SUCCESS
            ));
        } else {
            connection.send(new LogoutResponseS2CPacket(
                LogoutResponseS2CPacket.Status.ERROR
            ));
        }
    }

    /**
     * Called when the client verifies their account
     */
    @Override
    public void onVerify(VerifyC2SPacket packet) {
        if (packet.getCode().equalsIgnoreCase(connection.getVerificationCode())) {
            connection.onVerifySuccess();
        } else {
            connection.send(new VerifyFailedS2CPacket());
        }
    }
}
