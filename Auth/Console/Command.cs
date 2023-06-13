namespace Auth.Console;

public class Command
{
	public const string Prefix = "-";
	public string Short { get; init; } = string.Empty;
	public string Long { get; init; } = string.Empty;
	public int Argc { get; init; }
	public string Description { get; init; } = string.Empty;
	public string? Default { get; init; }
	public Command[]? SubCommands { get; init; }
	public Action<string[]> Action { get; init; } = _ => { };
}