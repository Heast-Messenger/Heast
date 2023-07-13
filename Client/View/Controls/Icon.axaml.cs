using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;

namespace Client.View.Controls;

public class Icon : TemplatedControl
{
	public static readonly DirectProperty<Icon, string?> TextProperty =
		AvaloniaProperty.RegisterDirect<Icon, string?>(nameof(Text),
			o => o.Text, (o, v) => o.Text = v);

	public static readonly StyledProperty<string?> SourceProperty =
		AvaloniaProperty.Register<Icon, string?>(nameof(Source));

	public static readonly StyledProperty<Orientation> OrientationProperty =
		AvaloniaProperty.Register<Icon, Orientation>(nameof(Orientation));

	public static readonly StyledProperty<double> IconSizeProperty =
		AvaloniaProperty.Register<Icon, double>(nameof(IconSize), 18.0);

	private string? _text;

	public double IconSize
	{
		get => GetValue(IconSizeProperty);
		set => SetValue(IconSizeProperty, value);
	}

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