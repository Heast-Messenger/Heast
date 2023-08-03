using Core.Services;

namespace Auth.Services;

public class TwoFactorService : ITwoFactorService
{
    private const string Charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public TwoFactorService(ILoggingService logger)
    {
        Logger = logger;
    }

    private ILoggingService Logger { get; }
    private Random Random { get; } = new();

    public Task<bool> Initialize()
    {
        Logger.Info($"{GetType().Name}.POST");
        return Task.FromResult(true);
    }

    public string GetVerificationCode()
    {
        const int length = 5;
        return new string(Enumerable.Repeat(Charset, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }

    public string GetVerificationUrl()
    {
        throw new NotImplementedException();
    }
}