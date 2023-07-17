using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;

namespace Client.View.Sidebars.Templates;

public class TitleTemplate : ContentControl
{
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<TitleTemplate, string?>(nameof(Title));

    public static readonly StyledProperty<InlineCollection> SubtitleProperty =
        AvaloniaProperty.Register<TitleTemplate, InlineCollection>(nameof(Subtitle));

    public TitleTemplate()
    {
        Subtitle = new InlineCollection();
    }

    public InlineCollection Subtitle
    {
        get => GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
}