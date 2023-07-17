using Avalonia;

namespace Client.Extensions;

public static class VersionExtension
{
    public static AppBuilder WithVersion(this AppBuilder builder, int major, int minor, int patch)
    {
        App.Version = $"{major}.{minor}.{patch}";
        return builder;
    }
}