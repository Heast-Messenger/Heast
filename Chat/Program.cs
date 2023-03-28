using System.Collections;
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

        /* Testing 
        bool[] bits = { true, true, true, false, false, true };
        
        PermissionsEngine.CreateRole("Admin", 1, new BitArray(bits));
        int rid = Database.GetRoleByName("Admin").PermissionRoleId;
        int uid = PermissionsEngine.CreateUser(rid);

        Console.Error.WriteLine(PermissionsEngine.HasPermission(uid, 2));

        Console.Error.WriteLine($"Sucessfully set Permissions? {PermissionsEngine.SetPermission(rid, 2, false)}");
        
        Console.Error.WriteLine(PermissionsEngine.HasPermission(uid, 2));
        */

        var logic = new EventLogic();
        logic.OnConnect += PermissionsEngine.UserConnect;
    }
}

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
        => services.AddDbContext<PermissionContext>();
}

