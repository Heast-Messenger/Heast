package heast.authserver.components;

import heast.authserver.Server;
import heast.core.logging.IO;

import javax.mail.*;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeMessage;
import java.io.IOException;
import java.net.URL;
import java.nio.file.Files;
import java.nio.file.Path;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.util.Map;
import java.util.Objects;
import java.util.function.Supplier;

public final class Email {
    private static final CodeSupply supply = new CodeSupply();
    private static Session session;
    private static String sender;

    private static final URL email = Objects.requireNonNull(
        Server.class.getResource("/heast/authserver/network/email.html"));

    public static void initialize() {
        IO.info.println("Initializing email service...");
        try {
            var parts = Files.readString(
                Path.of("/Users/fabian/Documents/Very Secure Folder/email-connection.txt")
            ).split(",");

            sender = parts[0];

            var properties = System.getProperties();
            properties.putAll(Map.of(
                "mail.smtp.host", parts[2],
                "mail.smtp.port", "465",
                "mail.smtp.auth", "true",
                "mail.smtp.ssl.enable", "true"
            ));

            session = Session.getInstance(properties, new Authenticator() {
                public PasswordAuthentication getPasswordAuthentication() {
                    return new PasswordAuthentication(sender, parts[1]);
                }
            });
        } catch (IOException e) {
            IO.error.println("Could not read database-email.txt");
        }
    }

    public static void send(String recipient, String code) {
        try {
            var mail = new MimeMessage(session);
            mail.setFrom(new InternetAddress(sender));
            mail.setRecipient(Message.RecipientType.TO, new InternetAddress(recipient));
            mail.setSubject("Your verification code!");
            mail.setText(Files.readString(Path.of(email.toURI()))
                .replace("{{ code }}", code)
                .replace("{{ date }}", LocalDate.now().format(DateTimeFormatter.ofPattern("dd.MM.yyyy"))),
                "UTF-8",
                "html"
            );
            Transport.send(mail);
            IO.info.println("Sending email to " + recipient + "...");
        } catch (IOException e) {
            IO.error.println("Could not read email template");
        } catch (Exception e) {
            IO.error.println("Could not send email to " + recipient);
        }
    }

    public static String getCode() {
        return supply.get();
    }

    private static class CodeSupply implements Supplier<String> {
        private static final String CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        @Override
        public String get() {
            var sb = new StringBuilder();
            for (int i = 0; i < 6; i++) {
                sb.append(CHARS.charAt((int) (Math.random() * CHARS.length())));
            }
            return sb.toString();
        }
    }
}
