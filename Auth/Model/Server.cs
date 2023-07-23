using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Model;

[Table("servers")]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class Server
{
    [Key] public int Id { get; }
    public required string Ip { get; init; }
    public required Status Status { get; init; }
    [InverseProperty("AddedServers")] public virtual required List<Account> Members { get; init; } = new();
}

public enum Status
{
    Normal,
    Warn,
    Banned
}