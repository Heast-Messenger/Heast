using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Model;

[Table("accounts")]
public class User
{
	[Key]
	public int Id { get; private set; }
	[MaxLength(255)]
	public string Name { get; set; }
	[MaxLength(255)]
	public string Email { get; set; }
	//[]
	public string Password { get; set; }

	public User(string name, string email, string password)
	{
		Name = name;
		Email = email;
		Password = password;
	}
}