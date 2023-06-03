using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Model;

[Table("servers")]
public class Server
{
    [Key]
    public int Id { get; private set; }
    public string Ip { get; set; }
    public Status Status { get; set; }

    public Server(string ip, Status status)
    {
        Ip = ip;
        Status = status;
    }
}

public enum Status
{
    Normal, Warn, Banned
}