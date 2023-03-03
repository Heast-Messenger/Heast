using System.ComponentModel.DataAnnotations.Schema;
using ChatServer.events;
using ChatServer.network;
using ChatServer.permissionengine;
using Microsoft.Extensions.DependencyInjection;

namespace ChatServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            Database.Init();
            PermissionsEngine.Init();

            EventLogic logic = new EventLogic();
            logic.OnConnect += PermissionsEngine.UserConnect;
        }
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
            => services.AddDbContext<PermissionContext>();
    }
}

