package heast.authserver.network;

import heast.core.network.UserAccount;
import heast.core.network.c2s.*;
import heast.core.network.c2s.listener.ServerAuthListener;
import heast.core.network.ClientConnection;
import heast.core.network.s2c.*;
import heast.core.utility.Validator;

public final class ServerAuthHandler implements ServerAuthListener {

    private final ClientConnection connection;

    public ServerAuthHandler(ClientConnection connection) {
        this.connection = connection;
    }

    @Override
    public void onLogin(LoginC2SPacket buf) {
        buf.decrypt(ServerNetwork.getServerKey().getPrivateKey(),ServerNetwork.getServerKey().getModulus());//decrypts packet
        String email = buf.getEmail();
        String password = buf.getPassword();

        if (!Validator.isEmailValid(email) ||
            !Validator.isPasswordValid(password)
        ) {
            connection.send(new LoginResponseS2CPacket(
                LoginResponseS2CPacket.Status.INVALID_CREDENTIALS, null
            ));
            return;
        }

        boolean userExists = Database.checkEntry(email);
        if (!userExists) {
            connection.send(new LoginResponseS2CPacket(
                LoginResponseS2CPacket.Status.USER_NOT_FOUND, null)
            );
            return;
        }

        ServerNetwork.sendVerificationCode(
            connection, email, () -> connection.send(new LoginResponseS2CPacket(
                LoginResponseS2CPacket.Status.CODE_SENT, null
            )), () -> {
                UserAccount user = ServerNetwork.loginClient(email, password);
                if (user != null) {
                    ServerNetwork.mapConnection(user.getId(), connection);
                    connection.send(new LoginResponseS2CPacket(
                        LoginResponseS2CPacket.Status.OK, user
                    ));
                } else {
                    connection.send(new LoginResponseS2CPacket(
                        LoginResponseS2CPacket.Status.INVALID_CREDENTIALS, null
                    ));
                }
            }
        );
    }

    @Override
    public void onSignup(SignupC2SPacket buf) {
        buf.decrypt(ServerNetwork.getServerKey().getPrivateKey(),ServerNetwork.getServerKey().getModulus());//decrypts packet
        String uname = buf.getUsername();
        String email = buf.getEmail();
        String password = buf.getPassword();

        if (!Validator.isUsernameValid(uname) ||
            !Validator.isEmailValid(email) ||
            !Validator.isPasswordValid(password)
        ) {
            connection.send(new SignupResponseS2CPacket(
                SignupResponseS2CPacket.Status.INVALID_CREDENTIALS
            ));
            return;
        }

        boolean userExists = Database.checkEntry(email);
        if (userExists) {
            connection.send(new SignupResponseS2CPacket(
                SignupResponseS2CPacket.Status.USER_EXISTS
            ));
            return;
        }

        ServerNetwork.sendVerificationCode(
            connection, email, () -> connection.send(new SignupResponseS2CPacket(
                SignupResponseS2CPacket.Status.CODE_SENT
            )), () -> {
                boolean successful = ServerNetwork.registerClient(uname, email, password);
                if (successful) {
                    connection.send(new SignupResponseS2CPacket(
                        SignupResponseS2CPacket.Status.OK
                    ));
                } else {
                    connection.send(new SignupResponseS2CPacket(
                        SignupResponseS2CPacket.Status.USER_EXISTS
                    ));
                }
            }
        );
    }

    @Override
    public void onDeleteAc(DeleteAcC2SPacket buf) {
        buf.decrypt(ServerNetwork.getServerKey().getPrivateKey(),ServerNetwork.getServerKey().getModulus());//decrypts packet
        String email = buf.getEmail();

        if (!Validator.isEmailValid(email)) {
            connection.send(new DeleteAcResponseS2CPacket(
                DeleteAcResponseS2CPacket.Status.INVALID_CREDENTIALS
            ));
            return;
        }

        boolean userExists = Database.checkEntry(email);
        if (!userExists) {
            connection.send(new DeleteAcResponseS2CPacket(
                DeleteAcResponseS2CPacket.Status.USER_NOT_FOUND
            ));
            return;
        }


        ServerNetwork.sendVerificationCode(
                connection, email, () -> connection.send(new DeleteAcResponseS2CPacket(
                    DeleteAcResponseS2CPacket.Status.CODE_SENT
                )), () -> {
                    boolean successful = ServerNetwork.deleteClient(email);
                    if (successful) {
                        connection.send(new DeleteAcResponseS2CPacket(
                                DeleteAcResponseS2CPacket.Status.OK
                        ));
                    } else {
                        connection.send(new DeleteAcResponseS2CPacket(
                                DeleteAcResponseS2CPacket.Status.USER_NOT_FOUND
                        ));
                    }
                }
        );
    }

    @Override
    public void onReset(ResetC2SPacket buf) {
        buf.decrypt(ServerNetwork.getServerKey().getPrivateKey(),ServerNetwork.getServerKey().getModulus());//decrypts packet
        String email = buf.getEmail();
        String newPassword = buf.getNewPassword();

        if (!Validator.isEmailValid(email) ||
            !Validator.isPasswordValid(newPassword)
        ) {
            connection.send(new ResetResponseS2CPacket(
                ResetResponseS2CPacket.Status.INVALID_CREDENTIALS
            ));
            return;
        }

        boolean userExists = Database.checkEntry(email);
        if (!userExists) {
            connection.send(new ResetResponseS2CPacket(
                ResetResponseS2CPacket.Status.USER_NOT_FOUND
            ));
            return;
        }

        ServerNetwork.sendVerificationCode(
            connection, email, () -> connection.send(new ResetResponseS2CPacket(
                ResetResponseS2CPacket.Status.CODE_SENT
            )), () -> {
                boolean successful = ServerNetwork.resetClientPassword(email, newPassword);
                if (successful) {
                    connection.send(new ResetResponseS2CPacket(
                        ResetResponseS2CPacket.Status.OK
                    ));
                } else {
                    connection.send(new ResetResponseS2CPacket(
                        ResetResponseS2CPacket.Status.FAILED
                    ));
                }
            }
        );
    }

    @Override
    public void onLogout(LogoutC2SPacket buf) {
        if(ServerNetwork.removeConnection(connection)) {
            System.out.println("Client logged out, reason: " + buf.getReason());
        } else {
            System.err.println("Client logout failed.");
        }
    }

    @Override
    public void onVerification(VerificationC2SPacket buf) {
        if (connection.getVerificationCode() != null) {
            buf.decrypt(ServerNetwork.getServerKey().getPrivateKey(), ServerNetwork.getServerKey().getModulus()); //decrypts packet
            String verificationCode = buf.getVerificationCode();
            if (connection.getVerificationCode().equals(verificationCode)) {
                System.out.println("Verification successful");
                connection.onVerificationCode();
            } else {
                System.err.println("Verification failed");
            }
        }
    }

    @Override
    public void onServerKeyRequest(ServerKeyC2SPacket buf) {
        System.out.println("Requesting server key");
        connection.send(
            new ServerKeyResponseS2CPacket(
                ServerNetwork.getServerKey().getPublicKey(),
                ServerNetwork.getServerKey().getModulus()
            )
        );
    }
}
