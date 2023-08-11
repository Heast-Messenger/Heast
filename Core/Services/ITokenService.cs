namespace Core.Services;

public interface ITokenService : IService
{
    byte[] Generate();
}