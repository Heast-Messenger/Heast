using Chat.events;
using Chat.Modules;
using Chat.Permissionengine;
using Chat.Structure;
using Microsoft.Extensions.DependencyInjection;

namespace Chat;

public static class Program
{
    public static void Main(string[] args)
    {
        Database.Init();
        PermissionsEngine.Init();

        var logic = new EventLogic();
        logic.OnConnect += PermissionsEngine.UserConnect;
    }
}

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
        => services.AddDbContext<PermissionContext>();
}

