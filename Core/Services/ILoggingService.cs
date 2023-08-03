namespace Core.Services;

public interface ILoggingService : IService
{
    void Debug(object data);
    void Info(object data);
    void Warn(object data);
    void Error(object data);
}