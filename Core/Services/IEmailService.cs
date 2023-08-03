namespace Core.Services;

public interface IEmailService : IService
{
    void SendSignupCode(string recipient, string username, string code);
    void SendLoginCode(string recipient, string username, string code);
    void SendResetCode(string recipient, string username, string code);
    void SendDeleteCode(string recipient, string username, string code);
}