using System.Security.Cryptography;
using System.Text;
using Core.Services;

namespace Auth.Services;

public class HashingService : IHashingService
{
    private const int KeySize = 128;
    private const int Iterations = 350000;

    public HashingService(ILoggingService logger)
    {
        Logger = logger;
    }

    private ILoggingService Logger { get; }
    private HashAlgorithmName HashAlgorithm { get; } = HashAlgorithmName.SHA512;

    public Task<bool> Initialize()
    {
        Logger.Info($"{GetType().Name}.POST");
        var random = new Random().Next().ToString("X");
        var hash = Hash(random, out var salt);
        var valid = Verify(random, hash, salt);
        return Task.FromResult(valid);
    }

    public byte[] Hash(string password, out byte[] salt)
    {
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