namespace Core.Services;

public interface IHashingService : IService
{
    byte[] Hash(string password, out byte[] salt);
    bool Verify(string password, byte[] hash, byte[] salt);
}