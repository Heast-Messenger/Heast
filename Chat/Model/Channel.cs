using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Model;

[Table("channels")]
public class Channel
{
    [Key]
    public int ChannelId { get; set; }
    public string Name { get; set; }
    
    public ChannelType Type { get; set; }

    public Channel(string name, ChannelType type)
    {
        Type = type;
        Name = name;
    }
}