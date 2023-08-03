namespace Core.Services;

public interface ITwoFactorService : IService
{
    string GetVerificationCode();
    string GetVerificationUrl();
}