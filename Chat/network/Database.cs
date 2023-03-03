

using System.Collections;
using ChatServer.permissionengine;
using ChatServer.util;

namespace ChatServer.network;

public class Database
{
    
    private static PermissionContext ctx { get; set; }
    
    public static void Init()
    {
        //TODO no fixed string
        ctx = new PermissionContext();
        LogUtil.SerializeAndFormat(BitArrayUtil.CascadeBitArrayList(GetPermissionListOfClient(1)));
    }

    public static PermissionClient GetClientById(int id)
    {
        return ctx.Clients.First(x => x.PermissionClientId == id);
    }

    public static PermissionRole GetRoleById(int id)
    {
        return ctx.Roles.First(x => x.PermissionRoleId == id);
    }

    public static PermissionChannel GetChannelById(int id)
    {
        return ctx.Channels.First(x => x.PermissionChannelId == id);
    }

    public static BitArray GetRolePermissionsOfChannel(int cid, int rid)
    {
        return (from a in ctx.ChannelPermissions
            where a.PermissionChannelId == cid && a.PermissionRoleId == rid
            select a.Permissions).First();
    }

    public static List<BitArray> GetPermissionListOfClient(int id)
    {
        var e = (from a in ctx.ClientRoles
            where a.PermissionClientId == id
            orderby a.PermissionRole.Hierarchy descending
            select a.PermissionRole.Permissions).ToList();
        return e;
    }

    public static List<int> GetRoleListOfClient(int id)
    {
        return (from a in ctx.ClientRoles
            where a.PermissionClientId == id
            select a.PermissionRoleId).ToList();
    }
    
    //TODO TEST IF IT WORKS
    public static List<BitArray> GetUserRolePermissionListOfChannel(int uid, int cid)
    {
        var b = GetRoleListOfClient(uid);
        
        var e = (from a in ctx.ChannelPermissions
            join u in b on a.PermissionRoleId equals u 
            where a.PermissionChannelId == cid
            orderby a.PermissionRole.Hierarchy descending
            select a.PermissionRole.Permissions).ToList();
        return e;
    }

}