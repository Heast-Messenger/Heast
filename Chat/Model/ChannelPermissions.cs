using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Model;

[Table("channelpermissions")]
public class ChannelPermissions
{
    [Key]
    public int ChannelId { get; set; }
    public Channel Channel { get; set; }
    
    [Key]
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