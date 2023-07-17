using System.Runtime.InteropServices;
using Auth.Structure;

namespace Auth;

public static class Global
{
    public const string Version = "1.0.0";
    public const string Build = "Dev 0.1";
    public const string Website = "https://heast.net/"; // TODO: Fetch from server to prevent outdated links
    public const string Github = "https://github.com/Heast-Messenger/Heast";

    public static string DbConnectionPath = "Assets/Database/Connection.txt";
    public static dynamic Translation = Translations.Load("en-us");

    public static string DotNetInfo => Environment.Version.ToString();
    public static string OsInfo => $"{RuntimeInformation.OSDescription} ({RuntimeInformation.OSArchitecture})";
}