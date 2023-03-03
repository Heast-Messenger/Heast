

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

    public static PermissionClient GetClientById(int id)
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
}