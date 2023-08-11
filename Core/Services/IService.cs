namespace Core.Services;

public interface IService
{
    const string Post = "Power on self test";
    Task<bool> Initialize();
}