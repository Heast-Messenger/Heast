using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;

namespace ChatServer.network;

public class Database
{

    //TODO better logs
    //TODO add configuration
    private static string connString = "Host=localhost;Port=5432;Username=admin;Password=;Database=heast";
    private static NpgsqlConnection conn;
    
    public static void Init()
    {
        CreateConnection();
        //Console.WriteLine(TestConnection().Result);
        CreateDatabase();
        CreateTables();
    }

    /// <summary>
    ///  Creates a connection to the database
    /// </summary>
    private static async void CreateConnection()
    {
        await using var dataSource = NpgsqlDataSource.Create(connString); ;

        conn = dataSource.OpenConnectionAsync().Result;
        conn.Notice += (_, args) => Console.WriteLine(args.Notice.MessageText);
            
        Console.WriteLine(conn.State == System.Data.ConnectionState.Open
            ? "Connection to database established"
            : "Connection to database failed");
    }

    /// <summary>
    /// Tests the connection to the database
    /// </summary>
    /// <returns></returns>
    private static async Task<bool> TestConnection()
    {
        await using var command = new NpgsqlCommand("SELECT '1'", conn);
        await using var reader = await command.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            return reader.GetString(0) == "1";
        }

        return false;
    }

    private static async Task<bool> DatabaseExists(string name)
    {
        await using var cmd = new NpgsqlCommand("SELECT 1 FROM pg_catalog.pg_database WHERE lower(datname) = lower(@name);", conn);
        cmd.Parameters.AddWithValue("@name", name);
        cmd.PrepareAsync();
        
        await using var reader = await cmd.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            return reader.GetInt32(0) == 1;
        }

        return false;
    }
    
    private static async void CreateDatabase()
    {
        if (await DatabaseExists("heast"))
        {
            Console.WriteLine("Database already exists, skipping creation");
            return;
        }
        
        await using var command = new NpgsqlCommand("CREATE DATABASE heast", conn);
        await command.ExecuteNonQueryAsync();

        Console.WriteLine("Database created");
    }
    
    
    /**
     * Creates a new database, if there is none
     */
    private static async void CreateTables()
    {
        //Default Tables
        await using var cmd = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS r_roles(" +
                                                "r_id int primary key," +
                                                "r_name varchar(255)," +
                                                "r_permissions bytea," +
                                                "r_hierarchy int)", conn);
        await cmd.ExecuteNonQueryAsync();
        
        await using var cmd2 = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS u_users(" +
                                                 "u_id int primary key," +
                                                 "u_name varchar(255)", conn);
        await cmd2.ExecuteNonQueryAsync();
        
        await using var cmd3 = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS ch_channels(" +
                                                 "ch_id int primary key," +
                                                 "ch_name varchar(255)", conn);
        await cmd3.ExecuteNonQueryAsync();
        
        //N-M Tables
        
        await using var cmd4 = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS ur_hasroles(" +
                                                "ur_u_id int," +
                                                "ur_r_id int," +
                                                "primary key(ur_u_id, ur_r_id)", conn);
        await cmd4.ExecuteNonQueryAsync();
        
        await using var cmd5 = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS c_channelpermissions(" +
                                                 "c_ch_id int," +
                                                 "c_r_id int," +
                                                 "c_permissions bytea," +
                                                 "primary key(ur_u_id, ur_r_id)", conn);
        await cmd5.ExecuteNonQueryAsync();
    }

    public static void ShutdownGracefully()
    {
        
    }
}