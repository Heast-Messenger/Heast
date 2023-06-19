using Auth.Modules;
using Auth.Structure;
using Microsoft.Extensions.DependencyInjection;

namespace Auth;

public static class Program
{
	public static Dispatcher Dispatcher { get; } = new Dispatcher();
    public static void Main(string[] args)
    {
	    try
        {
            Dispatcher.Dispatch(args);
        }
        catch (Exception e)
        {
            Dispatcher.Crash(e);
        }
    }
}

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
        => services.AddDbContext<AuthContext>();
}