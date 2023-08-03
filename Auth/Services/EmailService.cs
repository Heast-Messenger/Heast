using System.Net;
using System.Net.Mail;
using Auth.Configuration;
using Core.Services;

namespace Auth.Services;

public class EmailService : IEmailService
{
    public EmailService(EmailConfig emailConfig, ILoggingService logger)
    {
        EmailConfig = emailConfig;
        Logger = logger;
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

    private ILoggingService Logger { get; }
    private EmailConfig EmailConfig { get; }
    private SmtpClient SmtpClient { get; }

    private string SignupVerificationTemplate { get; } = File.ReadAllText("Assets/Email/SignupVerification.html");

    public Task<bool> Initialize()
    {
        Logger.Info($"{GetType().Name}.POST");
        var tcs = new TaskCompletionSource<bool>();
        SmtpClient.SendCompleted += (sender, args) =>
        {
            tcs.SetResult(true);
        };
        try
        {
            SmtpClient.SendAsync(
                EmailConfig.Username,
                EmailConfig.Username,
                "Power On Self Test",
                "This is a power on self test",
                userToken: null);

            return tcs.Task;
        }
        catch (Exception)
        {
            return Task.FromResult(false);
        }
    }

    public void SendSignupCode(string recipient, string username, string code)
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

    public void SendLoginCode(string recipient, string username, string code)
    {
        throw new NotImplementedException();
    }

    public void SendResetCode(string recipient, string username, string code)
    {
        throw new NotImplementedException();
    }

    public void SendDeleteCode(string recipient, string username, string code)
    {
        throw new NotImplementedException();
    }
}