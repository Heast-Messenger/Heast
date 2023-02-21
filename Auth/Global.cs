using Auth.Structure;

namespace Auth;

public static class Global
{
	public const string Version = "1.0.0";
	public const string Build = "Dev 0.1";
	public const string Website = "https://heast.net/"; // TODO: Fetch from server to prevent outdated links
	public const string Github = "https://github.com/Heast-Messenger/Heast";

	public static string DotNetInfo => Environment.Version.ToString();
	public static string OsInfo => $"{System.Runtime.InteropServices.RuntimeInformation.OSDescription} ({System.Runtime.InteropServices.RuntimeInformation.OSArchitecture})";

	public static string DbConnectionPath = "Assets/Database/Connection.txt";
	public static Translation Translation = Translation.Load("en-us");
}