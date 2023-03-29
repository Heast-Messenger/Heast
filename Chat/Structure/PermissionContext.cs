using Chat.Model;
using Microsoft.EntityFrameworkCore;

namespace Chat.Structure;

public class PermissionContext : DbContext
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Channel> Channels => Set<Channel>();
    public DbSet<ChannelPermissions> ChannelPermissions => Set<ChannelPermissions>();
    public DbSet<ClientRoles> ClientRoles => Set<ClientRoles>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder
            .UseNpgsql("Host=localhost;Port=5432;Username=admin;Password=k0nradius;Database=heast")
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();
        //.LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.UseIdentityAlwaysColumns();
        
        modelBuilder.Entity<Role>().ToTable("roles");
        modelBuilder.Entity<Client>().ToTable("clients");
        modelBuilder.Entity<Channel>().ToTable("channels");
        modelBuilder.Entity<ClientRoles>().HasKey(sc => new { PermissionRoleId = sc.RoleId, PermissionClientId = sc.ClientId });
        modelBuilder.Entity<ChannelPermissions>().HasKey(sc => new { PermissionRoleId = sc.RoleId, PermissionChannelId = sc.ChannelId });

        modelBuilder.Entity<Channel>().Property(c => c.ChannelId)
            .ValueGeneratedOnAdd()
            .HasColumnType("integer");
        modelBuilder.Entity<Role>().Property(r => r.RoleId)
            .ValueGeneratedOnAdd()
            .HasColumnType("integer");;
        modelBuilder.Entity<Client>().Property(c => c.ClientId)
            .ValueGeneratedOnAdd()
            .HasColumnType("integer");

    }
}