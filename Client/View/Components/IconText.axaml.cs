using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace Client.View.Components;

public partial class IconText : UserControl
{
	public static readonly DirectProperty<IconText, Control?> TextProperty =
		AvaloniaProperty.RegisterDirect<IconText, Control?>(nameof(Text),
			o => o.Text, (o, v) => o.Text = v);

	public static readonly StyledProperty<IImage?> IconProperty =
		AvaloniaProperty.Register<IconText, IImage?>(nameof(Icon));

	public static readonly StyledProperty<Orientation> OrientationProperty =
		AvaloniaProperty.Register<IconText, Orientation>(nameof(Orientation));

	private Control? _text;

	public IconText()
	{
		InitializeComponent();
	}

	public Control? Text
	{
		get => _text;
		set => SetAndRaise(TextProperty, ref _text, value);
	}

	public IImage? Icon
	{
		get => GetValue(IconProperty);
		set => SetValue(IconProperty, value);
	}

	public Orientation Orientation
	{
		get => GetValue(OrientationProperty);
		set => SetValue(OrientationProperty, value);
	}
}