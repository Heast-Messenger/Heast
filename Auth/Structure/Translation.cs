using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Auth.Structure;

public class Translation
{
	public string ArgsHelpDescription { get; set; } = string.Empty;
	public string ArgsHelpHelp { get; set; } = string.Empty;
	public string ArgsHelpVersion { get; set; } = string.Empty;
	public string ArgsHelpStop { get; set; } = string.Empty;
	public string ArgsHelpStart { get; set; } = string.Empty;
	public string ArgsHelpStartIp { get; set; } = string.Empty;
	public string ArgsHelpStartPort { get; set; } = string.Empty;
	public string ArgsHelpStartDbhost { get; set; } = string.Empty;
	public string ArgsHelpStartDbport { get; set; } = string.Empty;
	public string ArgsUnknown { get; set; } = string.Empty;

	public string ArgsHelpVersionVersion { get; set; } = string.Empty;
	public string ArgsHelpVersionBuild { get; set; } = string.Empty;
	public string ArgsHelpVersionWebsite { get; set; } = string.Empty;
	public string ArgsHelpVersionGithub { get; set; } = string.Empty;
	public string ArgsHelpVersionDotnet { get; set; } = string.Empty;
	public string ArgsHelpVersionOs { get; set; } = string.Empty;

	public string ServerStarting { get; set; } = string.Empty;
	public string ServerStarted { get; set; } = string.Empty;
	public string ServerNoinstances { get; set; } = string.Empty;
	public string ServerExiting { get; set; } = string.Empty;

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
			throw new($"No suitable language '{name}' could be found/loaded: {e.Message}");
		}
	}
}