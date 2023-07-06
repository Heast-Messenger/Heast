using Avalonia;
using Avalonia.Controls.Primitives;

namespace Client.View.Controls;

public class Icon : TemplatedControl
{
	public static readonly StyledProperty<string> SourceProperty = AvaloniaProperty.Register<Icon, string>(
		"Source");

	public string Source
	{
		get => GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}
}