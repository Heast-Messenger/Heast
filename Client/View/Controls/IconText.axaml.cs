using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;

namespace Client.View.Controls;

public class IconText : TemplatedControl
{
	public static readonly DirectProperty<IconText, string?> TextProperty =
		AvaloniaProperty.RegisterDirect<IconText, string?>(nameof(Text),
			o => o.Text, (o, v) => o.Text = v);

	public static readonly StyledProperty<string?> SourceProperty =
		AvaloniaProperty.Register<IconText, string?>(nameof(Source));

	public static readonly StyledProperty<Orientation> OrientationProperty =
		AvaloniaProperty.Register<IconText, Orientation>(nameof(Orientation));

	private string? _text;

	public string? Text
	{
		get => _text;
		set => SetAndRaise(TextProperty, ref _text, value);
	}

	public string? Source
	{
		get => GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}

	public Orientation Orientation
	{
		get => GetValue(OrientationProperty);
		set => SetValue(OrientationProperty, value);
	}
}