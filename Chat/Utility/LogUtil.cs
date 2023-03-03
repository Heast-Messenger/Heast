using Newtonsoft.Json;

namespace Chat.Utility;

public class LogUtil
{
    public static void SerializeAndFormat(object b)
    {
        Console.WriteLine(JsonConvert.SerializeObject(b, new JsonSerializerSettings
        {
            Formatting = Formatting.Indented
        }));
    }
}