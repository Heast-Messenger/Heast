package heast.authserver.components;

import heast.core.logging.IO;
import heast.core.model.UserAccount;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.sql.*;

public final class Database {

    private static Connection connection;

    /**
     * Initializes the database.
     */
    public static void initialize() {
        IO.info.println("Initializing database...");
        try {
            Class.forName("com.mysql.cj.jdbc.Driver");
        } catch (ClassNotFoundException e) {
            IO.error.println("MySQL driver not found");
        }

        try {
            var parts = Files.readString(
                Path.of("/Users/fabian/Documents/Very Secure Folder/database-connection.txt")
            ).split(",");

            try {
                connection = DriverManager.getConnection(
                    "jdbc:mysql://" + parts[0] + ":" + parts[1] + "/" + parts[2] + "?user=" + parts[3] + "&password=" + parts[4]
                );

                createDatabase();
            } catch (SQLException e) {
                IO.error.println("Failed to connect to database");
            }
        } catch (IOException e) {
            IO.error.println("Could not read database-connection.txt");
        }
    }

    /**
     * Creates a new database if there is none
     */
    private static void createDatabase() {
        try {
            var stmt = connection.createStatement();
            stmt.execute("CREATE DATABASE IF NOT EXISTS messenger");
            stmt.execute("USE messenger");
            stmt.execute("CREATE TABLE IF NOT EXISTS accounts (" +
                "id INT(16) UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY UNIQUE," +
                "name VARCHAR(32) NOT NULL," +
                "email VARCHAR(255) NOT NULL," +
                "password VARCHAR(255) NOT NULL" +
            ");");
        } catch (SQLException e) {
            IO.error.println("Failed to create database");
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
            var stmt = connection.prepareStatement("INSERT INTO accounts (name, email, password) VALUES (?, ?, ?)");
            stmt.setString(1, name);
            stmt.setString(2, email);
            stmt.setString(3, password);
            stmt.execute();
            return true;
        } catch (SQLException e) {
            IO.error.println("Failed to add entry to database for " + email);
            return false;
        }
    }

    /**
     * Removes a user from the database.
     * @param email The email address of the user.
     * @return Whether the user was successfully removed.
     */
    public static boolean removeEntry(String email){
        try {
            var stmt = connection.prepareStatement("DELETE FROM accounts WHERE email = ?");
            stmt.setString(1,email);
            stmt.execute();
            return true;
        } catch (SQLException e) {
            IO.error.println("Failed to delete entry from database for " + email);
            return false;
        }
    }

    /**
     * Checks if a user exists in the database.
     * @param email The email to be checked.
     * @param password The password to be checked.
     */
    public static boolean userExists(String email, String password) {
        try {
            var stmt = connection.prepareStatement("SELECT * FROM accounts WHERE email = ? AND password = ?");
            stmt.setString(1, email);
            stmt.setString(2, password);
            ResultSet result = stmt.executeQuery();
            return result.next();
        } catch (SQLException e) {
            IO.error.println("No entry found in database for " + email);
            return false;
        }
    }

    /**
     * Checks if a user exists in the database.
     * @param email The email to be checked.
     */
    public static boolean userExists(String email) {
        try {
            var stmt = connection.prepareStatement("SELECT * FROM accounts WHERE email = ?");
            stmt.setString(1, email);
            ResultSet result = stmt.executeQuery();
            return result.next();
        } catch (SQLException e) {
            IO.error.println("No entry found in database for " + email);
            return false;
        }
    }

    /**
     * Gets a user in the database
     * @param email the user's email address used to identify the row in the table
     * @return the user's account information
     */
    public static UserAccount getUser(String email) {
        try {
            var stmt = connection.prepareStatement("SELECT * FROM accounts WHERE email = ?");
            stmt.setString(1, email);
            ResultSet result = stmt.executeQuery();
            if (result.next()) {
                return new UserAccount(
                    result.getInt("id"),
                    result.getString("name"),
                    result.getString("email"),
                    result.getString("password")
                );
            } else {
                return null;
            }
        } catch (SQLException e) {
            IO.error.println("No entry found in database for " + email);
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
            var stmt = connection.prepareStatement("UPDATE accounts SET password = ? WHERE email = ?");
            stmt.setString(1, password);
            stmt.setString(2, email);
            stmt.execute();
            return true;
        } catch (SQLException e) {
            IO.error.println("Failed to update password for " + email);
            return false;
        }
    }
}
