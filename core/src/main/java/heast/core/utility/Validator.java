package heast.core.utility;

public final class Validator {
    public static boolean isUsernameValid(String name) {
        return name.matches("[a-zA-Z\\d]{3,16}");
    }

    public static boolean isEmailValid(String email) {
        return email.matches("^[a-zA-Z\\d._-]+@[a-zA-Z\\d.-]+\\.[a-zA-Z]{2,4}$");
    }

    public static boolean isPasswordValid(String password) {
        return password.matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\\d)(?=.*?[#?!@$%^&*-]).{8,}$");
    }

    public static boolean isVerificationCodeValid(String verificationCode) {
        return verificationCode.matches("[A-Z\\d]{6}");
    }

    public static boolean isIpAddressValid(String ipAddress) {
        return ipAddress.matches("(\\b25[0-5]|\\b2[0-4][0-9]|\\b[01]?[0-9][0-9]?)(\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}") || ipAddress.equals("localhost");
    }

    public static boolean isPortValid(int port) {
        return port >= 0 && port <= 65535;
    }
}
