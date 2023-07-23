using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Model;

[Table("sessions")]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class Session
{
    [Key] public int Id { get; }
    public required string Token { get; init; }
    public virtual required Account Account { get; init; }
    public required DateTime Created { get; init; }
    public required DateTime Expires { get; init; }

    public bool IsExpired()
    {
        return DateTime.Now > Expires;
    }
}