

using System.Collections;
using Chat.Model;
using Chat.Permissionengine;
using Chat.Structure;
using Chat.Utility;

namespace Chat.Modules;

public static class Database {

    //TODO private
    public static PermissionContext Ctx { get; set; } = null!;
    
    public static void Init()
    {
        //TODO no fixed string
        Ctx = new PermissionContext();
    }

    //Querying
    public static PermissionClient GetUserById(int id)
    {
        return Ctx.Clients.First(x => x.PermissionClientId == id);
    }

    public static PermissionRole GetRoleById(int id)
    {
        return Ctx.Roles.First(x => x.PermissionRoleId == id);
    }
    public static PermissionRole GetRoleByName(string name)
    {
        return Ctx.Roles.First(x => x.Name.Equals(name));
    }

    public static PermissionChannel GetChannelById(int id)
    {
        return Ctx.Channels.First(x => x.PermissionChannelId == id);
    }

    private static int GetHighestChannelId()
    {
        if (!Ctx.Channels.Any()) return 0;
        return Ctx.Channels.Max(x => x.PermissionChannelId);
    }
    
    private static int GetHighestRoleId()
    {
        if (!Ctx.Roles.Any()) return 0;
        return Ctx.Roles.Max(x => x.PermissionRoleId);
    }
    
    private static int GetHighestUserId()
    {
        if (!Ctx.Clients.Any()) return 0;
        return Ctx.Clients.Max(x => x.PermissionClientId);
    }
    
    public static BitArray GetRolePermissionsOfChannel(int cid, int rid)
    {
        return (from a in Ctx.ChannelPermissions
            where a.PermissionChannelId == cid && a.PermissionRoleId == rid
            select a.Permissions).First();
    }

    public static List<BitArray> GetPermissionListOfClient(int id)
    {
        var e = (from a in Ctx.ClientRoles
            join y in Ctx.Roles on a.PermissionRoleId equals y.PermissionRoleId
            where a.PermissionClientId == id
            orderby a.PermissionRole.Hierarchy descending
            select y.Permissions).ToList();
        return e;
    }

    public static List<int> GetClientsWithRole(int rid)
    {
        return (from a in Ctx.ClientRoles
            where a.PermissionRoleId == rid
            select a.PermissionClientId).ToList();
    }

    public static List<int> GetRoleIdListOfClient(int id)
    {
        return (from a in Ctx.ClientRoles
            where a.PermissionClientId == id
            select a.PermissionRoleId).ToList();
    }
    
    
    public static List<BitArray> GetUserRolePermissionListOfChannel(int uid, int cid)
    {
        var b = GetRoleIdListOfClient(uid);
        
        return (from a in Ctx.ChannelPermissions
            join u in b on a.PermissionRoleId equals u 
            where a.PermissionChannelId == cid
            orderby a.PermissionRole.Hierarchy descending
            select a.PermissionRole.Permissions).ToList();
            
    }

    // Map implementation
    public static int CreateChannel(string name)
    {
        var id = GetHighestChannelId() + 1;
        Ctx.Channels.Add(new PermissionChannel(name, id));
        Ctx.SaveChanges();

        return id;
    }
    
    public static int CreateRole(string name, int hierarchy, BitArray permissions)
    {
        var id = GetHighestRoleId() + 1;
        Ctx.Roles.Add(new PermissionRole(name, id, hierarchy, permissions));
        Ctx.SaveChanges();

        return id;
    }

    public static int CreateUser()
    {
        var id = GetHighestUserId() + 1;
        Ctx.Clients.Add(new PermissionClient(id));
        Ctx.SaveChanges();
        
        return id;
    }

    //TODO config
    public static int CreateUser(int role)
    {
        var id = GetHighestUserId() + 1;
        Ctx.Clients.Add(new PermissionClient(id));
        Ctx.ClientRoles.Add(new PermissionClientRoles(id, role));
        Ctx.SaveChanges();
        
        return id;
    }
    
    public static bool SetPermission(int rid, int pid, bool value)
    {
        if (!Ctx.Roles.Any(x => x.PermissionRoleId == rid)) return false;

        BitArray e = Ctx.Roles.First(x => x.PermissionRoleId == rid).Permissions;
        e[pid] = value;
        Ctx.Roles.First(x => x.PermissionRoleId == rid).Permissions = new BitArray(BitArrayUtil.ConvertToBoolArray(e));
        
        Ctx.SaveChanges();
        return true;
    }
    
    public static bool SetRole(int uid, int rid, bool value)
    {
        if (!Ctx.Clients.Any(x => x.PermissionClientId == uid)) return false;
        if (!Ctx.Roles.Any(x => x.PermissionRoleId == rid)) return false;
        if (value && Ctx.ClientRoles.Any(x => x.PermissionRoleId == rid && x.PermissionClientId == uid)) return false;

        if (value) Ctx.ClientRoles.Add(new PermissionClientRoles(uid, rid));
        else Ctx.ClientRoles.Remove(new PermissionClientRoles(uid, rid));
        
        Ctx.SaveChanges();

        return true;
    }
    
    public static bool SetChannelPermission(int cid, int rid, int pid, bool value)
    {
        if (!Ctx.Channels.Any(x => x.PermissionChannelId == cid)) return false;
        if (!Ctx.Roles.Any(x => x.PermissionRoleId == rid)) return false;
        if (Ctx.ChannelPermissions.Any(x => x.PermissionRoleId == rid && x.PermissionChannelId == cid))
        {
            Ctx.ChannelPermissions.First(x => x.PermissionRoleId == rid && x.PermissionChannelId == cid)
                .Permissions[pid] = value;
            return true;
        }

        if (!value) return false;
        bool[] perms = new bool[PermissionsEngine.ChannelPermissionMaxSize];
        perms[pid] = true;

        Ctx.ChannelPermissions.Add(new PermissionChannelPermissions(cid, rid, new BitArray(perms)));
        return true;
    }

    
}