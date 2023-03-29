

using System.Collections;
using System.Text.Json;
using Chat.Model;
using Chat.Permissionengine;
using Chat.Structure;
using Chat.Utility;

namespace Chat.Modules;

public class Database : IDatabase {

    private PermissionContext Ctx { get; set; } = null!;
    
    public Database()
    {
        //TODO no fixed string
        Ctx = new PermissionContext();
    }

    //Querying
    public Client GetClientById(int id)
    {
        return Ctx.Clients.First(x => x.ClientId == id);
    }

    public Role GetRoleById(int id)
    {
        return Ctx.Roles.First(x => x.RoleId == id);
    }
    public Role GetRoleByName(string name)
    {
        return Ctx.Roles.First(x => x.Name.Equals(name));
    }

    public Channel GetChannelById(int id)
    {
        return Ctx.Channels.First(x => x.ChannelId == id);
    }

    private int GetHighestChannelId()
    {
        if (!Ctx.Channels.Any()) return 0;
        return Ctx.Channels.Max(x => x.ChannelId);
    }
    
    private int GetHighestRoleId()
    {
        if (!Ctx.Roles.Any()) return 0;
        return Ctx.Roles.Max(x => x.RoleId);
    }
    
    private int GetHighestClientId()
    {
        if (!Ctx.Clients.Any()) return 0;
        return Ctx.Clients.Max(x => x.ClientId);
    }
    
    public BitArray GetRolePermissionInChannel(int cid, int rid)
    {
        return (from a in Ctx.ChannelPermissions
            where a.ChannelId == cid && a.RoleId == rid
            select a.Permissions).First();
    }

    public List<BitArray> GetPermissionListOfClient(int id)
    {
        var e = (from a in Ctx.ClientRoles
            join y in Ctx.Roles on a.RoleId equals y.RoleId
            where a.ClientId == id
            orderby a.Role.Hierarchy descending
            select y.Permissions).ToList();
        return e;
    }

    public List<int> GetClientsWithRole(int rid)
    {
        return (from a in Ctx.ClientRoles
            where a.RoleId == rid
            select a.ClientId).ToList();
    }

    public List<int> GetRoleIdListOfClient(int id)
    {
        return (from a in Ctx.ClientRoles
            where a.ClientId == id
            select a.RoleId).ToList();
    }
    
    
    public List<BitArray> GetClientPermissionsOfChannel(int uid, int cid)
    {
        var b = GetRoleIdListOfClient(uid);
        
        return (from a in Ctx.ChannelPermissions
            join u in b on a.RoleId equals u 
            where a.ChannelId == cid
            orderby a.Role.Hierarchy descending
            select a.Role.Permissions).ToList();
            
    }

    public int GetHierarchy(int clientId)
    {
        return Ctx.ClientRoles.Where(x => x.ClientId == clientId).Min(x => x.Role.Hierarchy);
    }

    // Map implementation
    public int CreateChannel(string name, ChannelType type)
    {
        Channel c = new Channel(name,type);
        Ctx.Channels.Add(c);
        Ctx.SaveChanges();

        return c.ChannelId;
    }
    
    public int CreateRole(string name, int hierarchy, BitArray permissions)
    {
        Role r = new Role(name, hierarchy, permissions);
        Ctx.Roles.Add(r);
        Ctx.SaveChanges();

        return r.RoleId;
    }

    public int CreateClient()
    {
        Client c = new Client();
        Ctx.Clients.Add(c);
        Ctx.SaveChanges();
        
        return c.ClientId;
    }

    public bool SetPermission(int rid, int pid, bool value)
    {
        if (!Ctx.Roles.Any(x => x.RoleId == rid)) return false;

        BitArray e = Ctx.Roles.First(x => x.RoleId == rid).Permissions;
        e[pid] = value;
        Ctx.Roles.First(x => x.RoleId == rid).Permissions = new BitArray(BitArrayUtil.ConvertToBoolArray(e));
        
        Ctx.SaveChanges();
        return true;
    }
    
    public bool SetRole(int uid, int rid, bool value)
    {
        if (!Ctx.Clients.Any(x => x.ClientId == uid)) return false;
        if (!Ctx.Roles.Any(x => x.RoleId == rid)) return false;
        if (value && Ctx.ClientRoles.Any(x => x.RoleId == rid && x.ClientId == uid)) return false;

        if (value) Ctx.ClientRoles.Add(new ClientRoles(uid, rid));
        else Ctx.ClientRoles.Remove(new ClientRoles(uid, rid));
        
        Ctx.SaveChanges();

        return true;
    }
    
    public bool SetChannelPermission(int cid, int rid, int pid, bool value)
    {
        if (!Ctx.Channels.Any(x => x.ChannelId == cid)) return false;
        if (!Ctx.Roles.Any(x => x.RoleId == rid)) return false;
        if (Ctx.ChannelPermissions.Any(x => x.RoleId == rid && x.ChannelId == cid))
        {
            BitArray e = Ctx.ChannelPermissions.First(x => x.ChannelId == cid).Permissions;
            e[pid] = value;
            Ctx.ChannelPermissions.First(x => x.RoleId == rid && x.ChannelId == cid)
                .Permissions = new BitArray(BitArrayUtil.ConvertToBoolArray(e));
            return true;
        }

        if (!value) return false;
        bool[] perms = new bool[PermissionsEngine.ChannelPermissionMaxSize];
        perms[pid] = true;

        Ctx.ChannelPermissions.Add(new ChannelPermissions(cid, rid, new BitArray(perms)));
        return true;
    }

    
}