namespace Core.Services;

public interface IEmailService : IService
{
    public void SendSignupCode(string recipient, string username, string code, string blockLink);
    void SendLoginCode(string recipient, string username, string code);
    void SendResetCode(string recipient, string username, string code);
    void SendDeleteCode(string recipient, string username, string code);
    string GenerateBlockLink(string email);
}