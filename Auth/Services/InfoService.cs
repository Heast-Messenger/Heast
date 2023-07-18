using System.Runtime.InteropServices;
using Auth.Structure;

namespace Auth.Services;

public class InfoService
{
    public string Build = "Dev 0.1";

    public string DbConnectionPath = "Assets/Database/Connection.txt";
    public string Github = "https://github.com/Heast-Messenger/Heast";
    public dynamic Translation = Translations.Load("en-us");
    public string Version = "1.0.0";
    public string Website = "https://heast.net/"; // TODO: Fetch from server to prevent outdated links

    public string DotNetInfo => Environment.Version.ToString();
    public string OsInfo => $"{RuntimeInformation.OSDescription} ({RuntimeInformation.OSArchitecture})";
}