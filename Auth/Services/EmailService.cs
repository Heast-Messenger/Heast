using System.Net;
using System.Net.Mail;
using Auth.Configuration;
using Core.Services;
using Microsoft.Extensions.Logging;

namespace Auth.Services;

public class EmailService : IEmailService
{
    public EmailService(EmailConfig emailConfig, ILogger<EmailService> logger)
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

    private ILogger<EmailService> Logger { get; }
    private EmailConfig EmailConfig { get; }
    private SmtpClient SmtpClient { get; }

    private string SignupVerificationTemplate { get; } = File.ReadAllText("Assets/Email/SignupVerification.html");

    public Task<bool> Initialize()
    {
        Logger.LogInformation(IService.Post);
        var tcs = new TaskCompletionSource<bool>();
        SmtpClient.SendCompleted += (sender, args) =>
        {
            tcs.SetResult(true);
        };
        try
        {
            SendEmail(EmailConfig.Username,
                "Power On Self Test",
                "This is a power on self test");

            return tcs.Task;
        }
        catch (Exception)
        {
            return Task.FromResult(false);
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

    public string GenerateBlockLink(string email)
    {
        return "'blockLink'";
    }

    public void SendSignupCode(string recipient, string username, string code, string blockLink)
    {
        var body = SignupVerificationTemplate
            .Replace("{{Username}}", username)
            .Replace("{{Code}}", code)
            .Replace("{{BlockLink}}", blockLink);
        SendEmail(recipient, $"Your verification code is: {code}", body);
    }

    private void SendEmail(string recipient, string subject, string body)
    {
        Logger.LogDebug("Sending email to {}", recipient);
        using (var mailMessage = new MailMessage())
        {
            mailMessage.From = new MailAddress(EmailConfig.Username);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            mailMessage.To.Add(recipient);
            SmtpClient.Send(mailMessage);
        }
    }
}