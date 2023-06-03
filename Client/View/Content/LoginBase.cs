using Avalonia.Controls;

namespace Client.View.Content;

public abstract class LoginBase : UserControl
{
	public abstract LoginBase? Back { get; }
	public abstract double? WindowHeight { get; }
}