using System.ComponentModel.DataAnnotations.Schema;
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
        }
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
            => services.AddDbContext<PermissionContext>();
    }
}

