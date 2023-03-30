using System.Collections;
using System.Diagnostics;
using Chat.events;
using Chat.Model;
using Chat.Modules;
using Chat.Permissionengine;
using Chat.Permissionengine.Permissions.Identifiers;
using Chat.Structure;
using Microsoft.Extensions.DependencyInjection;

namespace Chat;

public static class Program
{
    public static void Main(string[] args)
    {
        PermissionsEngine.Init();
        PermissionsEngine.InitEventSystem(new EventLogic());


        bool[] bits = new bool[PermissionsEngine.RolePermissionMaxSize];
        
        int modId = PermissionsEngine.CreateRole("Mod", 2, new BitArray(bits));
        bits[1] = true;
        int adminId = PermissionsEngine.CreateRole("Admin", 1, new BitArray(bits));
        int clientId = PermissionsEngine.CreateClient(adminId);

        int adminclient = PermissionsEngine.CreateClient(adminId);
        int modclient = PermissionsEngine.CreateClient(modId);
        
        
        Console.WriteLine($"(t) Has the user permission {(int) PermissionIdentifier.Ban}: {PermissionsEngine.HasPermission(clientId, (int) PermissionIdentifier.Ban)}");

        PermissionsEngine.SetPermission(adminId, (int) PermissionIdentifier.Ban, false);
        Console.WriteLine($"(f) Has the user permission {(int) PermissionIdentifier.Ban} after mod: {PermissionsEngine.HasPermission(clientId, (int) PermissionIdentifier.Ban)}");

        PermissionsEngine.SetRole(clientId, modId, true);
        Console.WriteLine($"(f) Has the user permission {(int) PermissionIdentifier.Ban} after rolechange: {PermissionsEngine.HasPermission(clientId, (int) PermissionIdentifier.Ban)}");

        PermissionsEngine.SetPermission(modId, (int)PermissionIdentifier.Ban, true);
        Console.WriteLine($"(t) Has the user permission {(int) PermissionIdentifier.Ban} after mod&rolechange: {PermissionsEngine.HasPermission(clientId, (int) PermissionIdentifier.Ban)}");

        Console.WriteLine($"({adminclient}) Is the Admin Client {adminclient} higher in the hierarchy than {modclient}: {PermissionsEngine.GetHigherClient(adminclient, adminclient)}");

        int channelId = PermissionsEngine.CreateChannel("name", ChannelType.File);
        PermissionsEngine.SetChannelPermission(channelId, adminId,
            (int) ChannelPermissionIdentifiers.See, true);
        
        Console.WriteLine($"(t) Can the user {adminclient} see {channelId} : {PermissionsEngine.CanSeeChannel(channelId, adminclient)}");
        
        PermissionsEngine.SetChannelPermission(channelId, adminId,
            (int) ChannelPermissionIdentifiers.See, false);
        
        Console.WriteLine($"(f) Can the user {adminclient} see {channelId} : {PermissionsEngine.CanSeeChannel(channelId, adminclient)}");

    }
}

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
        => services.AddDbContext<PermissionContext>();
}

