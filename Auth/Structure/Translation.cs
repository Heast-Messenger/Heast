using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Auth.Structure;

public class Translation
{
    public string ArgsHelpDescription { get; set; } = "";
    public string ArgsHelpHelp { get; set; } = "";
    public string ArgsHelpVersion { get; set; } = "";
    public string ArgsHelpStop { get; set; } = "";
    public string ArgsHelpStart { get; set; } = "";
    public string ArgsHelpStartIp { get; set; } = "";
    public string ArgsHelpStartPort { get; set; } = "";
    public string ArgsHelpStartDbhost { get; set; } = "";
    public string ArgsHelpStartDbport { get; set; } = "";
    public string ArgsUnknown { get; set; } = "";

    public string ArgsHelpVersionVersion { get; set; } = "";
    public string ArgsHelpVersionBuild { get; set; } = "";
    public string ArgsHelpVersionWebsite { get; set; } = "";
    public string ArgsHelpVersionGithub { get; set; } = "";
    public string ArgsHelpVersionDotnet { get; set; } = "";
    public string ArgsHelpVersionOs { get; set; } = "";

    public string ServerStarting { get; set; } = "";
    public string ServerStarted { get; set; } = "";
    public string ServerNoinstances { get; set; } = "";
    public string ServerExiting { get; set; } = "";

    public static Translation Load(string name)
    {
        try
        {
            var data = File.ReadAllText($"Assets/Lang/{name}.json");
            var translation = JsonConvert.DeserializeObject<Translation>(data, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new DotCaseNamingStrategy()
                }
            });
            if (translation == null) throw new JsonSerializationException("Could not deserialize translation");
            return translation;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception($"No suitable language '{name}' could be found/loaded: {e.Message}");
        }
    }
}