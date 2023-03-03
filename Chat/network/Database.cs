

using System.Collections;
using System.Xml;
using ChatServer.permissionengine;
using ChatServer.util;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ChatServer.network;

public class Database
{
    
    private static PermissionContext ctx { get; set; }
    
    public static void Init()
    {
        //TODO no fix string
        ctx = new PermissionContext();
        LogUtil.SerializeAndFormat(BitArrayUtil.CascadeBitArrayList(GetPermissionListOfClient(1)));
    }

    /*
    public static PermissionClient TestReadClient()
    {
        return getClientById(1);
    }

    public static void TestAddClient()
    {
        ctx.Clients.AddAsync(new PermissionClient("Gustav", 4));
        ctx.SaveChanges();
    }

    public static PermissionRole TestReadRole()
    {
        return ctx.Roles.ToList().First();
    }

    public static void TestAddRole()
    {
        ctx.Roles.AddAsync(new PermissionRole("Admin",
            1, 1,
            new BitArray(new bool[] { false, false, true })));
        ctx.SaveChanges();
    }

    public static void TestGetRoles()
    {
        var a = from e in ctx.Clients
            join f in ctx.ClientRoles on e.PermissionClientId equals f.PermissionClientId
            where f.PermissionClientId == 1
            select new { e.PermissionClientId, f.PermissionRole };
        
        
        Console.WriteLine(JsonConvert.SerializeObject(a, new JsonSerializerSettings
        {
            Formatting = Formatting.Indented
        }));
    }*/

    
    public static PermissionClient GetClientById(int id)
    {
        return ctx.Clients.First(x => x.PermissionClientId == id);
    }

    public static PermissionRole GetRoleById(int id)
    {
        return ctx.Roles.First(x => x.PermissionRoleId == id);
    }

    public static List<BitArray> GetPermissionListOfClient(int id)
    {
        var e = (from a in ctx.ClientRoles
            where a.PermissionClientId == id
            orderby a.PermissionRole.Hierarchy descending
            select a.PermissionRole.Permissions).ToList();
        return e;
    }

}