using System;
using System.Net.Mime;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Client.View.Components; 

public partial class SidebarButton : UserControl {
    
    public SidebarButton() {
        InitializeComponent();
    }
    
    // This is nearly identical to the TextBlock.TextProperty, but does not work here.
    // What to try out: Copy the TextBlock code here and set a breakpoint at the setter
    // of the Text property. The check if the value is being set, unlike now.
    
    // Edit: Tried replacing the code, but it stops working when I initialize the
    // component in the constructor. Why? Idk.
    // Edit 2: Now fixed. It's really 'geistig' (How I like to call it)
    public static readonly DirectProperty<SidebarButton, string?> TextProperty =
        AvaloniaProperty.RegisterDirect<SidebarButton, string?>(nameof(MediaTypeNames.Text),
            o => o.Text, (o, v) => o.Text = v);
    
    public static readonly StyledProperty<string> IconProperty =
        AvaloniaProperty.Register<SidebarButton, string>(nameof(Icon));
    
    public static readonly RoutedEvent<RoutedEventArgs> ClickEvent =
        RoutedEvent.Register<SidebarButton, RoutedEventArgs>(nameof(Click),
            RoutingStrategies.Bubble);

    private string? _text;
    
    public string? Text {
        get => _text;
        set => SetAndRaise(TextProperty, ref _text, value);
    }
    
    public string Icon {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
    
    public event EventHandler<RoutedEventArgs>? Click {
        add => AddHandler(ClickEvent, value);
        remove => RemoveHandler(ClickEvent, value);
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e) {
        RaiseEvent(new RoutedEventArgs(ClickEvent));
    }
}