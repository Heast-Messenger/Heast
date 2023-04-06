using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Client.View.Components;

public partial class IconText : UserControl
{
    public static readonly DirectProperty<IconText, string?> TextProperty =
        AvaloniaProperty.RegisterDirect<IconText, string?>(nameof(Text),
            o => o.Text, (o, v) => o.Text = v);

    public static readonly StyledProperty<IImage> IconProperty =
        AvaloniaProperty.Register<IconText, IImage>(nameof(Icon));

    private string? _text;

    public IconText()
    {
        InitializeComponent();
    }

    public string? Text
    {
        get => _text;
        set => SetAndRaise(TextProperty, ref _text, value);
    }

    public IImage Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
}
