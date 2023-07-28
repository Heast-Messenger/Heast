using System.Net;
using System.Net.Mail;
using Auth.Configuration;

namespace Auth.Services;

public class EmailService
{
    public EmailService(EmailConfig emailConfig)
    {
        EmailConfig = emailConfig;
        SmtpClient = new SmtpClient
        {
            Host = EmailConfig.Host,
            Port = EmailConfig.Port,
            Credentials = new NetworkCredential
            {
                UserName = EmailConfig.Username,
                Password = EmailConfig.Password
            },
            EnableSsl = true
        };
    }

    private EmailConfig EmailConfig { get; }
    private SmtpClient SmtpClient { get; }

    private string SignupVerificationTemplate { get; } = File.ReadAllText("Assets/Email/SignupVerification.html");

    public void SendCode(string recipient, string username, string code)
    {
        var body = SignupVerificationTemplate
            .Replace("{{Username}}", username)
            .Replace("{{Code}}", code)
            .Replace("{{BlockLink}}", "" /*TODO*/);

        using (var mailMessage = new MailMessage())
        {
            mailMessage.From = new MailAddress(EmailConfig.Username);
            mailMessage.Subject = $"Your verification code: {code}";
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            mailMessage.To.Add(recipient);
            SmtpClient.Send(mailMessage);
        }
    }
}