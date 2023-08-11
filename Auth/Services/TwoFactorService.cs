using Core.Services;
using Microsoft.Extensions.Logging;

namespace Auth.Services;

public class TwoFactorService : ITwoFactorService
{
    private const string Charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public TwoFactorService(ILogger<TwoFactorService> logger)
    {
        Logger = logger;
    }

    private ILogger<TwoFactorService> Logger { get; }
    private Random Random { get; } = new();

    public Task<bool> Initialize()
    {
        Logger.LogInformation(IService.Post);
        return Task.FromResult(true);
    }

    public string GetVerificationCode()
    {
        Logger.LogDebug("Generating verification code");
        const int length = 5;
        return new string(Enumerable.Repeat(Charset, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }

    public string GetVerificationUrl()
    {
        Logger.LogDebug("Generating verification url");
        throw new NotImplementedException();
    }
}