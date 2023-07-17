using Newtonsoft.Json;
using static System.Console;

namespace Auth.Structure;

public static class Translations
{
    public static dynamic Load(string name)
    {
        try
        {
            var data = File.ReadAllText($"Assets/Lang/{name}.json");
            var translation = JsonConvert.DeserializeObject<dynamic>(data);
            if (translation == null)
            {
                throw new JsonSerializationException("Could not deserialize translation");
            }

            return translation;
        }
        catch (Exception e)
        {
            WriteLine(e.Message);
            throw new Exception($"No suitable language '{name}' could be found/loaded: {e.Message}");
        }
    }
}