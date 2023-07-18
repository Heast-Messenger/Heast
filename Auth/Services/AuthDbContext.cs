using Auth.Model;
using Microsoft.EntityFrameworkCore;

namespace Auth.Services;

public class AuthDbContext : DbContext
{
    public IEnumerable<User> Accounts => Set<User>();
    public IEnumerable<Server> Servers => Set<Server>();
    public IEnumerable<Session> Sessions => Set<Session>();

    public static string Host { get; set; } = "localhost";
    public static int Port { get; set; } = 3306;

    public static string ConnectionString => File.ReadAllText("Assets/Database/Connection.txt")
        .Replace("{host}", Host)
        .Replace("{port}", Port.ToString())
        .Replace("{db}", "heast_auth");

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
        optionsBuilder.LogTo(WriteToFile);
        optionsBuilder.EnableDetailedErrors();
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

    public static void WriteToFile(string message)
    {
        File.AppendAllText("Assets/Database/Log.txt", message + Environment.NewLine);
    }

    public IEnumerable<User> GetUsers()
    {
        return Accounts.ToList();
    }
}