namespace Core.Server;

public class Command
{
    public required string Short { get; init; }
    public required string Long { get; init; }
    public int Argc { get; init; }
    public required string Description { get; init; }
    public string? Default { get; init; }
    public Command[]? SubCommands { get; init; }

    public required Action<string[]> Action { get; init; }
}