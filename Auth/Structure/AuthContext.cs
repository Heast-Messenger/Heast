using Auth.Model;
using static Auth.Modules.Database;
using Microsoft.EntityFrameworkCore;

namespace Auth.Structure;

public class AuthContext : DbContext
{
	public DbSet<User> Accounts => Set<User>();
	public DbSet<Server> Servers => Set<Server>();
	public DbSet<Session> Sessions => Set<Session>();

	private static string ConnectionString => File.ReadAllText("Assets/Database/Connection.txt")
		.Replace("{host}", Host)
		.Replace("{port}", Port.ToString())
		.Replace("{db}", "heast_auth");

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);
		var con = ConnectionString;
		optionsBuilder
			.UseMySql(con, ServerVersion.AutoDetect(con))
			.LogTo(Console.WriteLine)
			.EnableDetailedErrors()
			.EnableSensitiveDataLogging();
	}

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