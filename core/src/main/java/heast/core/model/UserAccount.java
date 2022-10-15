package heast.core.model;

public final class UserAccount {
    private final int id;
    private final String username;
    private final String email;
    private final String password;
    private final String avatar;

    public UserAccount(int id, String username, String email, String password) {
        this.id = id;
        this.username = username;
        this.email = email;
        this.password = password;
        this.avatar = "http://localhost:8080/avatars?id=" + id;
    }

    public int getId() {
        return id;
    }

    public String getUsername() {
        return username;
    }

    public String getEmail() {
        return email;
    }

    public String getPassword() {
        return password;
    }

    public String getAvatar() {
        return avatar;
    }

    @Override
    public String toString() {
        return "UserAccount{" + "id=" + id + ", username='" + username + '\'' + ", email='" + email + '\'' + ", password='" + password + '\'' + ", avatar='" + avatar + '\'' + '}';
    }
}