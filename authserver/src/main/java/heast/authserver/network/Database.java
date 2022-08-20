package heast.authserver.network;

import heast.core.network.UserAccount;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.sql.*;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

public final class Database {

    private static Connection connection;

    /**
     * Initializes the database.
     */
    public static void initialize() {
        System.out.println("Initializing database...");
        try {
            Class.forName("com.mysql.cj.jdbc.Driver");
        } catch (ClassNotFoundException e) {
            System.err.println("MySQL driver not found");
        }

        try {
            String[] parts = Files.readString(
                Path.of("C:\\Users\\Admin\\Documents\\Very Secure Folder\\database-connection.txt")
            ).split(",");

            try {
                connection = DriverManager.getConnection(
                    "jdbc:mysql://" + parts[0] + ":" + parts[1] + "/" + parts[2] + "?user=" + parts[3] + "&password=" + parts[4]
                );
            } catch (SQLException e) {
                System.err.println("Failed to connect to database");
            }
        } catch (IOException e) {
            System.err.println("Could not read database-connection.txt");
        }
    }

    /**
     * Adds a user to the database.
     * @param name The username to be registered.
     * @param email The email address of the user.
     * @param password The hashed password of the account.
     */
    public static boolean addEntry(String name, String email, String password) {    //TODO: Doesn't work if table is fully cleared?
        try {
            PreparedStatement stmt = connection.prepareStatement("INSERT INTO accounts (name, since, email, password) VALUES (?, ?, ?, ?)");
            stmt.setString(1, name);
            stmt.setString(2, LocalDateTime.now().format(
                DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss")
            ));
            stmt.setString(3, email);
            stmt.setString(4, password);
            stmt.execute();
            return true;
        } catch (SQLException e) {
            System.err.println("Failed to add entry to database for " + email);
            return false;
        }
    }

    public static boolean removeEntry(String email){
        try {
            PreparedStatement stmt = connection.prepareStatement("DELETE FROM accounts WHERE email = ?");
            stmt.setString(1,email);
            stmt.execute();
            return true;
        } catch (SQLException e) {
            System.err.println("Failed to delete entry from database for " + email);
            return false;
        }
    }

    /**
     * Checks if a user exists in the database.
     * @param email The email to be checked.
     * @param password The password to be checked.
     */
    public static boolean checkEntry(String email, String password) {
        try {
            PreparedStatement stmt = connection.prepareStatement("SELECT * FROM accounts WHERE email = ? AND password = ?");
            stmt.setString(1, email);
            stmt.setString(2, password);
            ResultSet result = stmt.executeQuery();
            return result.next();
        } catch (SQLException e) {
            System.err.println("No entry found in database for " + email);
            return false;
        }
    }

    /**
     * Checks if a user exists in the database.
     * @param email The email to be checked.
     */
    public static boolean checkEntry(String email) {
        try {
            PreparedStatement stmt = connection.prepareStatement("SELECT * FROM accounts WHERE email = ?");
            stmt.setString(1, email);
            ResultSet result = stmt.executeQuery();
            return result.next();
        } catch (SQLException e) {
            System.err.println("No entry found in database for " + email);
            return false;
        }
    }

    public static UserAccount getUser(String email) {
        try {
            PreparedStatement stmt = connection.prepareStatement("SELECT * FROM accounts WHERE email = ?");
            stmt.setString(1, email);
            ResultSet result = stmt.executeQuery();
            if (result.next()) {
                return new UserAccount(
                    result.getInt("id"),
                    result.getString("name"),
                    result.getString("email"),
                    result.getString("password"),
                    LocalDateTime.parse(result.getString("since"), DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss")),
                    null,
                    null,
                    null
                );
            } else {
                return null;
            }
        } catch (SQLException e) {
            System.err.println("No entry found in database for " + email);
            return null;
        }
    }

    /**
     * Updates the password for a user.
     * @param email The email address of the user.
     * @param password The new password.
     */
    public static boolean updatePassword(String email, String password) {
        try {
            PreparedStatement stmt = connection.prepareStatement("UPDATE accounts SET password = ? WHERE email = ?");
            stmt.setString(1, password);
            stmt.setString(2, email);
            stmt.execute();
            return true;
        } catch (SQLException e) {
            System.err.println("Failed to update password for " + email);
            return false;
        }
    }
}
