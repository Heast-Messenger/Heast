using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Model;

public class ChannelPermissions
{
    public int ChannelId { get; set; }
    public Channel Channel { get; set; }
    
    public int RoleId { get; set; }
    public Role Role { get; set; }
    public BitArray Permissions { get; set; }

    public ChannelPermissions(int channelId, int roleId, BitArray permissions)
    {
        ChannelId = channelId;
        RoleId = roleId;
        Permissions = permissions;
    }
}