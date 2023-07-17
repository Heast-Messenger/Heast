using Microsoft.Extensions.Configuration;

namespace Core.Utility;

public static partial class Shared
{
    static Shared()
    {
        Config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("Assets/appsettings.json")
            .Build();
    }

    public static IConfigurationRoot Config { get; }
}