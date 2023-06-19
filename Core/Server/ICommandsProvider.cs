namespace Core.Server;

public interface ICommandsProvider
{
	public Command[] List { get; }
}