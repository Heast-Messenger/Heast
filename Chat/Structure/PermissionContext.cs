using Chat.Model;
using Microsoft.EntityFrameworkCore;

namespace Chat.Structure;

public class PermissionContext : DbContext
{
    public DbSet<PermissionClient> Clients => Set<PermissionClient>();
    public DbSet<PermissionRole> Roles => Set<PermissionRole>();
    public DbSet<PermissionChannel> Channels => Set<PermissionChannel>();
    public DbSet<PermissionChannelPermissions> ChannelPermissions => Set<PermissionChannelPermissions>();
    public DbSet<PermissionClientRoles> ClientRoles => Set<PermissionClientRoles>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder
            .UseNpgsql("Host=localhost;Port=5432;Username=admin;Password=;Database=heast")
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<PermissionRole>().ToTable("permissionroles");
        modelBuilder.Entity<PermissionClient>().ToTable("permissionclients");;
        modelBuilder.Entity<PermissionChannel>().ToTable("permissionchannels");
        modelBuilder.Entity<PermissionClientRoles>().HasKey(sc => new { sc.PermissionRoleId, sc.PermissionClientId });
        modelBuilder.Entity<PermissionChannelPermissions>().HasKey(sc => new { sc.PermissionRoleId, sc.PermissionChannelId });
    }
}