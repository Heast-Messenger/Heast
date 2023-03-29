using System.Collections;

namespace Chat.Model;

public interface IDatabase
{
    public int CreateClient();

    public int CreateRole(string name, int hierarchy, BitArray permissions);

    public int CreateChannel(string name, ChannelType type);

    public Client GetClientById(int clientId);

    public BitArray GetRolePermissionInChannel(int channelId, int roleId);

    public List<int> GetClientsWithRole(int roleId);

    public bool SetPermission(int roleId, int permissionId, bool value);
    public bool SetRole(int clientId, int roleId, bool value);
    public bool SetChannelPermission(int channelId, int roleId, int permissionId, bool value);

    public List<BitArray> GetPermissionListOfClient(int clientId);

    public List<BitArray> GetClientPermissionsOfChannel(int clientId, int channelId);
    public int GetHierarchy(int clientId);
}