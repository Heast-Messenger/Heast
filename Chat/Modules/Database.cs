

using System.Collections;
using Chat.Model;
using Chat.Structure;

namespace Chat.Modules;

public static class Database {

    private static PermissionContext Ctx { get; set; } = null!;
    
    public static void Init()
    {
        //TODO no fixed string
        Ctx = new PermissionContext();
    }

    public static PermissionClient GetUserById(int id)
    {
        return Ctx.Clients.First(x => x.PermissionClientId == id);
    }

    public static PermissionRole GetRoleById(int id)
    {
        return Ctx.Roles.First(x => x.PermissionRoleId == id);
    }

    public static PermissionChannel GetChannelById(int id)
    {
        return Ctx.Channels.First(x => x.PermissionChannelId == id);
    }

    private static int GetHighestChannelId()
    {
        return Ctx.Channels.Max(x => x.PermissionChannelId);
    }
    
    private static int GetHighestRoleId()
    {
        return Ctx.Roles.Max(x => x.PermissionRoleId);
    }
    
    private static int GetHighestUserId()
    {
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
            where a.PermissionClientId == id
            orderby a.PermissionRole.Hierarchy descending
            select a.PermissionRole.Permissions).ToList();
        return e;
    }

    public static IEnumerable<int> GetRoleListOfClient(int id)
    {
        return (from a in Ctx.ClientRoles
            where a.PermissionClientId == id
            select a.PermissionRoleId).ToList();
    }
    
    //TODO TEST IF IT WORKS
    public static List<BitArray> GetUserRolePermissionListOfChannel(int uid, int cid)
    {
        var b = GetRoleListOfClient(uid);
        
        return (from a in Ctx.ChannelPermissions
            join u in b on a.PermissionRoleId equals u 
            where a.PermissionChannelId == cid
            orderby a.PermissionRole.Hierarchy descending
            select a.PermissionRole.Permissions).ToList();
    }

    
    //TODO Fallback when there is no highest channel id
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


}