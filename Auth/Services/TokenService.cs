using Core.Services;
using Microsoft.Extensions.Logging;

namespace Auth.Services;

public class TokenService : ITokenService
{
    public TokenService(ILogger<TokenService> logger)
    {
        Logger = logger;
    }

    private ILogger<TokenService> Logger { get; }

    public Task<bool> Initialize()
    {
        Logger.LogInformation(IService.Post);
        return Task.FromResult(true);
    }

    public byte[] Generate()
    {
        throw new NotImplementedException();
    }
}