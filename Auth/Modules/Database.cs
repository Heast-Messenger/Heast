using Auth.Model;
using Auth.Structure;
using static System.Console;

namespace Auth.Modules;

public static class Database
{
	public static AuthContext? Db { get; private set; }

	public static string Host { get; set; } = "localhost";
	public static int Port { get; set; } = 3306;

	private static string ConnectionString => File.ReadAllText("Assets/Database/Connection.txt")
		.Replace("{host}", Host)
		.Replace("{port}", Port.ToString())
		.Replace("{db}", "heast_auth");

	public static void Initialize()
	{
		WriteLine($"Initializing authentication database on {Host}:{Port}...");

		try
		{
			Db = new AuthContext(ConnectionString);
			WriteLine($"Database connected on {Host}:{Port}");
		}
		catch (Exception e)
		{
			if (Db is null)
			{
				throw new NullReferenceException(
					"The database connection returned null. " +
					"Are you sure you have a running MySQL instance with the correct connection properties?",
					e);
			}

			throw new Exception("The database connection failed.", e);
		}
	}

	public static void WriteToFile(string message)
	{
		File.AppendAllText("Assets/Database/Log.txt", message + Environment.NewLine);
	}

	public static IEnumerable<User> GetUsers()
	{
		if (Db is not null)
		{
			return Db.Accounts.ToList();
		}

		throw new NullReferenceException("Database connection is not available");
	}
}