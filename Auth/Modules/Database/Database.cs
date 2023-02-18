using Auth.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Modules.Database;

public static class Database {

    public static Context Db { get; private set; } = null!;
    public static string ConnectionString => File.ReadAllText("Assets/Connection")
        .Replace("{host}", Host)
        .Replace("{port}", Port.ToString())
        .Replace("{db}", "messenger");

    public static string Host { get; set; } = "localhost";
    public static int Port { get; set; } = 3306;

    public static void Initialize() {
        Console.WriteLine("Initializing authentication database...");
        var con = ConnectionString;
        // services.AddDbContext<Context>(db => db
        //     .UseMySql(con, ServerVersion.AutoDetect(con))
        //     //.LogTo(Console.WriteLine)
        //     .EnableDetailedErrors()
        //     .EnableSensitiveDataLogging());

        try {
            Db = new Context(new DbContextOptionsBuilder<Context>()
                .UseMySql(con, ServerVersion.AutoDetect(con))
                //.LogTo(Console.WriteLine)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .Options);
            Migrate();
        } finally {
            Db.Dispose();
        }
    }
    
    private static void Migrate() {
        Console.WriteLine("Migrating authentication database...");
        Db.Database.Migrate();
    }

    public static IEnumerable<User> GetUsers() {
        return Db.Accounts.ToList();
    }
}
