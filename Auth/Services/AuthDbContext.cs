using Auth.Model;
using Core.Utility;
using Microsoft.EntityFrameworkCore;

namespace Auth.Services;

public class AuthDbContext : DbContext
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Server> Servers => Set<Server>();
    public DbSet<Session> Sessions => Set<Session>();

    public static string Host { get; set; } = Shared.Config["db-host"]!;
    public static string Port { get; set; } = Shared.Config["db-port"]!;
    public static string Username { get; set; } = Shared.Config["db-user"]!;
    public static string Password { get; set; } = Shared.Config["db-password"]!;
    public static string Name { get; set; } = Shared.Config["db-name"]!;

    public static string ConnectionString => File.ReadAllText("Assets/Database/Connection.txt")
        .Replace("{host}", Host)
        .Replace("{port}", Port)
        .Replace("{user}", Username)
        .Replace("{password}", Password)
        .Replace("{db}", Name);

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(ConnectionString, new MySqlServerVersion(new Version()));
        optionsBuilder.LogTo(WriteToFile);
        optionsBuilder.EnableDetailedErrors();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Account>().ToTable("accounts").HasKey(s => s.Id);
        modelBuilder.Entity<Server>().ToTable("servers").HasKey(s => s.Id);
        modelBuilder.Entity<Session>().ToTable("sessions").HasKey(s => s.Id);

        modelBuilder.Entity<Account>(e =>
        {
            e.HasIndex(u => u.Name).IsUnique();
            e.HasIndex(u => u.Email).IsUnique();
        });
    }

    private static void WriteToFile(string message)
    {
        File.AppendAllText("Assets/Database/Log.txt", message + Environment.NewLine);
    }
}