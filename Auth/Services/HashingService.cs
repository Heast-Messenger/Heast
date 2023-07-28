using System.Security.Cryptography;
using System.Text;

namespace Auth.Services;

public class HashingService
{
    private const int KeySize = 128;
    private const int Iterations = 350000;
    private static HashAlgorithmName HashAlgorithm { get; } = HashAlgorithmName.SHA512;

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