using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Model;

[Table("accounts")]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class Account
{
    [Key] public int Id { get; }
    [MaxLength(255)] public required string Name { get; init; }
    [MaxLength(255)] public required string Email { get; init; }
    public required string Password { get; init; }
    [InverseProperty("Members")] public virtual required List<Server> AddedServers { get; init; } = new();
}