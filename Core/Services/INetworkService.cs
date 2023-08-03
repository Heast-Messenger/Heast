namespace Core.Services;

public interface INetworkService : IService
{
    void SetCertificate(string filepath);
}