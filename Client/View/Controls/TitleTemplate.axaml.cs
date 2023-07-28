using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;

namespace Client.View.Content.Login.Templates;

public class TitleTemplate : ContentControl
{
    public static readonly StyledProperty<InlineCollection> TitleProperty =
        AvaloniaProperty.Register<TitleTemplate, InlineCollection>(nameof(Title));

    public static readonly StyledProperty<InlineCollection> SubtitleProperty =
        AvaloniaProperty.Register<TitleTemplate, InlineCollection>(nameof(Subtitle));

    public TitleTemplate()
    {
        Title = new InlineCollection();
        Subtitle = new InlineCollection();
    }

    public InlineCollection Subtitle
    {
        get => GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public InlineCollection Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
}