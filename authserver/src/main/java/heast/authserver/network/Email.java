package heast.authserver.network;

import heast.authserver.Server;

import javax.mail.*;
import javax.mail.internet.AddressException;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeMessage;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.util.Map;
import java.util.Objects;
import java.util.Properties;
import java.util.function.Supplier;

public final class Email {
    private static final CodeSupply supply = new CodeSupply();
    private static Session session;
    private static String sender;

    public static void initialize() {
        System.out.println("Initializing email service...");
        try {
            String[] parts = Files.readString(
                Path.of("/Users/fabian/Documents/Very Secure Folder/email-connection.txt")
            ).split(",");

            sender = parts[0];

            Properties properties = System.getProperties();
            properties.putAll(Map.of(
                "mail.smtp.host", parts[2],
                "mail.smtp.port", "465",
                "mail.smtp.auth", "true",
                "mail.smtp.ssl.enable", "true"
            ));

            session = Session.getInstance(properties, new Authenticator() {
                @Override
                protected PasswordAuthentication getPasswordAuthentication() {
                    return new PasswordAuthentication(sender, parts[1]);
                }
            });
        } catch (IOException e) {
            System.err.println("Could not read database-email.txt");
        }
    }

    public static void send(String recipient, String code) {
        try {
            MimeMessage mail = new MimeMessage(session);
            mail.setFrom(new InternetAddress(sender));
            mail.setRecipient(Message.RecipientType.TO, new InternetAddress(recipient));
            mail.setSubject("Your verification code!");
            mail.setText(Files.readString(
                Path.of(Objects.requireNonNull(
                    Server.class.getResource("/heast/authserver/network/email minified.html")
                ).toURI())
            ).replace("{{ code }}", code
            ).replace("{{ date }}", LocalDate.now().format(DateTimeFormatter.ofPattern("dd.MM.yyyy"))
            ), "UTF-8", "html");

            System.out.println("Sending email to " + recipient + "...");
            Transport.send(mail);
        } catch (IOException e) {
            System.err.println("Could not read email template");
        } catch (Exception e) {
            System.err.println("Could not send email to " + recipient);
            e.printStackTrace();
        }
    }

    public static String getCode() {
        return supply.get();
    }

    private static class CodeSupply implements Supplier<String> {
        private static final String CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        @Override
        public String get() {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 6; i++) {
                sb.append(CHARS.charAt((int) (Math.random() * CHARS.length())));
            }
            return sb.toString();
        }
    }
}
