using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Model;

[Table("permissionchannels")]
public class PermissionChannel
{
    [Key]
    public int PermissionChannelId { get; set; }
    public string Name { get; set; }

    public PermissionChannel(string name, int permissionChannelId)
    {
        Name = name;
        PermissionChannelId = permissionChannelId;
    }
}