using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Model;

[Table("sessions")]
public class Session
{
    public Session(string token, int userId, DateTime created, DateTime expires)
    {
        Token = token;
        UserId = userId;
        Created = created;
        Expires = expires;
    }

    [Key] public int Id { get; }
    public string Token { get; set; }
    public int UserId { get; set; }
    public DateTime Created { get; set; }
    public DateTime Expires { get; set; }

    public bool IsExpired()
    {
        return DateTime.Now > Expires;
    }
}