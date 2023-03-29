using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Model;

[Table("clientroles")]
public class ClientRoles
{
    [Key]
    public int ClientId { get; set; }
    public Client Client { get; set; }
    
    [Key]
    public int RoleId { get; set; }
    public Role Role { get; set; }

    public ClientRoles(int clientId, int roleId)
    {
        ClientId = clientId;
        RoleId = roleId;
    }
}