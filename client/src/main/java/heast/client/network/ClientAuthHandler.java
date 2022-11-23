package heast.client.network;

import heast.core.logging.IO;
import heast.core.network.listeners.ClientAuthListener;
import heast.core.network.packets.s2c.auth.*;
import heast.core.network.pipeline.ClientConnection;

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
            }
            case AWAIT_VERIFICATION -> {
                IO.info.println("Awaiting verification");
            }
            case INVALID_CREDENTIALS -> {
                IO.error.println("Invalid credentials");
            }
            case USER_EXISTS -> {
                IO.error.println("User already exists");
            }
            case ERROR -> {
                IO.error.println("Unexpected error");
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
            }
            case AWAIT_VERIFICATION -> {
                IO.info.println("Awaiting verification");
            }
            case INVALID_CREDENTIALS -> {
                IO.error.println("Invalid credentials");
            }
            case USER_NOT_FOUND -> {
                IO.error.println("User does not exists");
            }
            case ERROR -> {
                IO.error.println("Unexpected error");
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
            }
            case AWAIT_VERIFICATION -> {
                IO.info.println("Awaiting verification");
            }
            case INVALID_CREDENTIALS -> {
                IO.error.println("Invalid credentials");
            }
            case USER_NOT_FOUND -> {
                IO.error.println("User does not exists");
            }
            case ERROR -> {
                IO.error.println("Unexpected error");
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
            }
            case AWAIT_VERIFICATION -> {
                IO.info.println("Awaiting verification");
            }
            case INVALID_EMAIL -> {
                IO.error.println("Invalid email");
            }
            case ERROR -> {
                IO.error.println("Unexpected error");
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
