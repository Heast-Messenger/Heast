using Auth.Configuration;
using Auth.Model;
using Microsoft.EntityFrameworkCore;

namespace Auth.Services;

public class AuthDbContext : DbContext
{
    public AuthDbContext(AuthDbConfig config)
    {
        Config = config;
    }

    public AuthDbContext(AuthDbConfig config, DbContextOptions<AuthDbContext> options)
        : base(options)
    {
        Config = config;
    }

    private AuthDbConfig Config { get; }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Server> Servers => Set<Server>();
    public DbSet<Session> Sessions => Set<Session>();

    public string ConnectionString => File.ReadAllText("Assets/Database/Connection.txt")
        .Replace("{host}", Config.Host)
        .Replace("{port}", Config.Port)
        .Replace("{user}", Config.Username)
        .Replace("{password}", Config.Password)
        .Replace("{db}", Config.Name);

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