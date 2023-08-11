using System.Security.Cryptography;
using System.Text;
using Core.Services;
using Microsoft.Extensions.Logging;

namespace Auth.Services;

public class HashingService : IHashingService
{
    private const int KeySize = 128;
    private const int Iterations = 350000;

    public HashingService(ILogger<HashingService> logger)
    {
        Logger = logger;
    }

    private ILogger<HashingService> Logger { get; }
    private HashAlgorithmName HashAlgorithm { get; } = HashAlgorithmName.SHA512;

    public Task<bool> Initialize()
    {
        Logger.LogInformation(IService.Post);
        var random = new Random().Next().ToString("X");
        var hash = Hash(random, out var salt);
        var valid = Verify(random, hash, salt);
        return Task.FromResult(valid);
    }

    public byte[] Hash(string password, out byte[] salt)
    {
        Logger.LogDebug("Hashing password {}", password);
        salt = RandomNumberGenerator.GetBytes(KeySize);
        return Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            Iterations,
            HashAlgorithm,
            KeySize);
    }

    public bool Verify(string password, byte[] hash, byte[] salt)
    {
        Logger.LogDebug("Verifying password {}", password);
        var compare = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            Iterations,
            HashAlgorithm,
            KeySize);

        return CryptographicOperations.FixedTimeEquals(
            compare,
            hash);
    }
}