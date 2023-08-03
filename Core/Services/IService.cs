namespace Core.Services;

public interface IService
{
    Task<bool> Initialize();
}