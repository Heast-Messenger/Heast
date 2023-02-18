using Auth.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Modules.Database;

public static class Database {

    public static Context Db { get; private set; } = null!;

    private static string ConnectionString => File.ReadAllText("Assets/Connection.txt")
        .Replace("{host}", Host)
        .Replace("{port}", Port.ToString())
        .Replace("{db}", "heast_auth");

    public static string Host { get; set; } = "localhost";
    public static int Port { get; set; } = 3306;

    public static async void Initialize() {
        Console.WriteLine("Initializing authentication database...");
        var con = ConnectionString;

        try {
            Db = new Context(new DbContextOptionsBuilder<Context>()
                .UseMySql(con, ServerVersion.AutoDetect(con))
                .LogTo(Console.WriteLine)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .Options);
            await Migrate();
        }
        catch (Exception e) {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (Db == null) {
                throw new Exception("The database connection returned null. Are you sure you have a running MySQL instance with the correct connection properties?", e);
            } else {
                throw new Exception("The database connection failed.", e);
            }
        }
        
        await Db.Accounts.AddAsync(new User("admin", "admin@gmail.com", "admin"));
        await Db.SaveChangesAsync();
    }
    
    private static async Task Migrate() {
        Console.WriteLine("Migrating authentication database...");

        var createScript = await File.ReadAllTextAsync("Assets/Create.sql");
        await Db.Database.EnsureCreatedAsync();
        await Db.Database.ExecuteSqlRawAsync(createScript);
        await Db.Database.MigrateAsync();
    }

    public static IEnumerable<User> GetUsers() {
        return Db.Accounts.ToList();
    }
}
