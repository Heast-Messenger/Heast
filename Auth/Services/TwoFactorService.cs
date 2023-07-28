namespace Auth.Services;

public class TwoFactorService
{
    // ReSharper disable StringLiteralTypo
    private const string Charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private Random Random { get; } = new();

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