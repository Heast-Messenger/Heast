using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace Client.View.Components;

public partial class SidebarButton : UserControl
{
    public static readonly DirectProperty<SidebarButton, string> TextProperty =
        AvaloniaProperty.RegisterDirect<SidebarButton, string>(nameof(Text),
            o => o.Text, (o, v) => o.Text = v);

    public static readonly StyledProperty<IImage> IconProperty =
        AvaloniaProperty.Register<SidebarButton, IImage>(nameof(Icon));

    public static readonly RoutedEvent<RoutedEventArgs> ClickEvent =
        RoutedEvent.Register<SidebarButton, RoutedEventArgs>(nameof(Click),
            RoutingStrategies.Bubble);

    private string? _text = string.Empty;

    public SidebarButton()
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

    public event EventHandler<RoutedEventArgs>? Click
    {
        add => AddHandler(ClickEvent, value);
        remove => RemoveHandler(ClickEvent, value);
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        RaiseEvent(new RoutedEventArgs(ClickEvent));
    }
}