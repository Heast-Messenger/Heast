using System.Collections;
using Chat.events;
using Chat.Model;
using Chat.Modules;
using Chat.Permissionengine.Permissions;
using Chat.Permissionengine.Permissions.Identifiers;
using Chat.Utility;
using Newtonsoft.Json;

namespace Chat.Permissionengine;

public class PermissionsEngine
{
    private static IDatabase Database = new Database();
    private static HashSet<Permission> _permissions = new();
    public static IReadOnlySet<Permission> Permissions => _permissions;
    private static Dictionary<int, BitArray> PriorityPermissions { get; } = new();

    private static EventLogic Logic { get; set; }
    
    public const int RolePermissionMaxSize = 255;
    public const int ChannelPermissionMaxSize = 512;

    public static void Init()
    {
        var text = File.ReadAllText("resources/permissions.json");
        _permissions = JsonConvert.DeserializeObject<HashSet<Permission>>(text)!;
        //TODO Create default roles (config)
        //TODO A way to send the user all available permissions when he enters the permission settings 
    }
    
    public static void InitEventSystem(EventLogic eventLogic)
    {
        Logic = eventLogic;
        Logic.OnConnect += ClientConnect;
        Logic.OnPermissionChanged += UpdatePriorityPermissions;
    }
    
    
    private static BitArray DeterminePriorityPermissions(int id)
    {
        return BitArrayUtil.CascadeBitArrayList(Database.GetPermissionListOfClient(id));
    }

    private static bool UpdatePriorityPermissions(int id)
    {
       /* 
        Console.WriteLine(JsonSerializer.Serialize(Database.Ctx.Roles.First(x => x.PermissionRoleId == 1).Permissions));
        Console.WriteLine("----------");
        Console.WriteLine(JsonSerializer.Serialize(Database.GetPermissionListOfClient(id)));
        Console.WriteLine("---------");
        Console.WriteLine(JsonSerializer.Serialize(DeterminePriorityPermissions(id)));
        */
        if (PriorityPermissions.ContainsKey(id))
        {
            PriorityPermissions[id] = DeterminePriorityPermissions(id);
            return true;
        }
        PriorityPermissions.Add(id, DeterminePriorityPermissions(id));
        return true;
    }

    /*Permission Querying*/
    public static bool HasPermission(int uid, int pid)
    {
        return PriorityPermissions[uid][pid];
    }

    public static bool SetChannelPermission(int cid, int rid, int pid, bool value)
    {
        return Database.SetChannelPermission(cid, rid, pid, value);
        //TODO maybe redetermine channelperms?
    }

    public static bool SetPermission(int rid, int pid, bool value)
    {
        var sol = Database.SetPermission(rid, pid, value);

        if (!sol) return sol;
        foreach (var client in Database.GetClientsWithRole(rid))
        {
            Logic.ClientUpdate(client);
        }

        return sol;
    }
    

    public static bool SetRole(int uid, int rid, bool value)
    {
        var sol = Database.SetRole(uid, rid, value);
        
        if (!sol) return sol;
        foreach (var client in Database.GetClientsWithRole(uid))
        {
            Logic.ClientUpdate(client);
        }

        return sol;
    }

    public static bool HasChannelPermissionForRole(int cid, int pid, int rid)
    {
        return Database.GetRolePermissionInChannel(cid, rid)[pid];
    }

    public static bool CanSeeChannel(int cid, int uid)
    {
        var b = BitArrayUtil.CascadeBitArrayList(Database.GetClientPermissionsOfChannel(uid, cid));
        return b[(int) ChannelPermissionIdentifiers.See];
    }

    /*Creation and Client connection*/
    public static void ClientConnect(int id)
    {
        PriorityPermissions.Add(id, Database.GetClientById(id) == null
                ? DeterminePriorityPermissions(Database.CreateClient())
                : DeterminePriorityPermissions(id));
        //TODO: send all roles per Client to client on connect
    }

    public static int CreateChannel(string name, ChannelType type)
    {
        return Database.CreateChannel(name, type);
    }

    public static int CreateRole(string name, int hierarchy, BitArray permissions)
    {
        return Database.CreateRole(name, hierarchy, permissions);
    }


    public static int CreateClientWithoutRole()
    {
        int id = Database.CreateClient();
        PriorityPermissions.Add(id, new BitArray(new bool[RolePermissionMaxSize]));
        return id;
    }
    
    public static int CreateClient()
    {
        return CreateClient(0);
        //TODO Default config
    }

    public static int CreateClient(int role)
    {
        int id = Database.CreateClient();
        SetRole(id, role, true);
        Logic.ClientUpdate(id);
        return id;
    }

    public static int GetHierarchy(int clientId)
    {
        return Database.GetHierarchy(clientId);
    }

    public static int GetHigherClient(int clientId1, int clientId2)
    {
        return GetHierarchy(clientId1) - GetHierarchy(clientId2) > 0 ? clientId2 : clientId1;
    }

    
}