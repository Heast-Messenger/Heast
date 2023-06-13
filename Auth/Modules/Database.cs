using Auth.Model;
using Auth.Structure;
using static System.Console;

namespace Auth.Modules;

public static class Database
{
	public static AuthContext Db { get; private set; } = null!;

	public static string Host { get; set; } = "localhost";
	public static int Port { get; set; } = 3306;

	public static void Initialize()
	{
		WriteLine("Initializing authentication database...");

		try
		{
			Db = new AuthContext();
			WriteLine($"Database connected on {Host}:{Port}");
		}
		catch (Exception e)
		{
			// ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
			if (Db == null)
			{
				throw new Exception(
					"The database connection returned null. Are you sure you have a running MySQL instance with the correct connection properties?",
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
		return Db.Accounts.ToList();
	}
}
