package heast.authserver.network;

import heast.authserver.components.Database;
import heast.authserver.components.Email;
import heast.authserver.security.PBKDF2;
import heast.core.logging.IO;
import heast.core.model.UserAccount;
import heast.core.network.pipeline.ClientConnection;
import heast.core.security.RSA;

import java.security.KeyPair;
import java.util.ArrayList;
import java.util.List;

public final class ServerNetwork {

    private static final List<ClientConnection> clients = new ArrayList<>();
    private static KeyPair keyPair;

    /**
     * Initializes the server by generating a new RSA keypair.
     */
    public static void initialize() {
        IO.info.println("Initializing server network...");
        keyPair = RSA.generateKeyPair();
    }

    /**
     * Gets the server's RSA keypair.
     */
    public static KeyPair getKeyPair() {
        return keyPair;
    }

    public static void addClient(ClientConnection connection) {
        if (!clients.contains(connection)) {
            clients.add(connection);
        } else {
            IO.error.println("Client connection already mapped");
        }
    }

    public static boolean removeClient(ClientConnection connection) {
        if (clients.contains(connection)) {
            clients.remove(connection);
            return true;
        } else {
            IO.error.println("Client connection not mapped");
            return false;
        }
    }

    public static void sendVerificationCode(ClientConnection connection, String email, Runnable onSent, Runnable onVerified) {
        var code = Email.getCode();
        connection.setVerificationCode(code);
        Email.send(email, code);
        onSent.run();
        connection.setOnVerifySuccess(onVerified);
    }

    public static boolean deleteClient(String email){
        IO.info.format("Account deletion request for %s%n", email);
        boolean successful = Database.removeEntry(email);
        if (successful) {
            IO.info.println(" -> Successfully deleted the account of " + email + "!");
            return true;
        } else {
            IO.error.println(" -> Failed to delete the account of " + email + "!");
            return false;
        }
    }

    public static boolean registerClient(String username, String email, String password) {
        IO.info.format("Signup request for %s with email %s and password %s%n", username, email, password);
        String hashedPassword = PBKDF2.hash(password.toCharArray());
        boolean successful = Database.addEntry(username, email, hashedPassword);
        if (successful) {
            IO.info.println(" -> Successfully registered user " + username + "!");
            return true;
        } else {
            IO.error.println(" -> Failed to register user " + username + "!");
            return false;
        }
    }

    public static UserAccount loginClient(String email, String password) {
        IO.info.format("Login request for %s with password %s%n", email, password);
        UserAccount user = Database.getUser(email);
        if (user != null && PBKDF2.verify(password.toCharArray(), user.getPassword())) {
            IO.info.println(" -> Successfully logged in user: " + user.getUsername() + "!");
            return user;
        } else {
            IO.error.println(" -> Failed to log in user " + email + "!");
            return null;
        }
    }

    public static boolean logoutClient(ClientConnection connection) {
        IO.info.format("Logout request for some client %n");
        return removeClient(connection);
    }

    public static boolean resetClientPassword(String email, String newPassword) {
        IO.info.format("Account reset request for %s%n", email);
        String hashedPassword = PBKDF2.hash(newPassword.toCharArray());
        boolean successful = Database.updatePassword(email, hashedPassword);
        if (successful) {
            IO.info.println(" -> Successfully reset password for " + email + "!");
            return true;
        } else {
            IO.error.println(" -> Failed to reset password for " + email + "!");
            return false;
        }
    }
}
