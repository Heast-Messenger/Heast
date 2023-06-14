using Auth.Model;
using Microsoft.EntityFrameworkCore;
using static Auth.Modules.Database;

namespace Auth.Structure;

public class AuthContext : DbContext
{
	public AuthContext(string con) : base(new DbContextOptionsBuilder()
		.UseMySql(con, ServerVersion.AutoDetect(con))
		.LogTo(WriteToFile)
		.EnableDetailedErrors()
		.Options)
	{
	}

	public DbSet<User> Accounts => Set<User>();
	public DbSet<Server> Servers => Set<Server>();
	public DbSet<Session> Sessions => Set<Session>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.Entity<User>().ToTable("accounts");
		modelBuilder.Entity<Server>().ToTable("servers");

		modelBuilder.Entity<User>(e =>
		{
			e.HasIndex(u => u.Name).IsUnique();
			e.HasIndex(u => u.Email).IsUnique();
		});
	}
}