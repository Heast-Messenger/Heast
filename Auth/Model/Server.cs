using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Model;

[Table("servers")]
public class Server
{
    public Server(string ip, Status status)
    {
        Ip = ip;
        Status = status;
    }

    [Key] public int Id { get; }
    public string Ip { get; set; }
    public Status Status { get; set; }
}

public enum Status
{
    Normal,
    Warn,
    Banned
}