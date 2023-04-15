using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;

namespace Client.View;

public partial class Titlebar : UserControl
{
    private readonly bool _isOsx = OperatingSystem.IsMacOS();
    private readonly bool _isWin = OperatingSystem.IsWindows();

    private readonly Rectangle _osxControlSpacer;
    private readonly Rectangle _winControlSpacer;

    public Titlebar()
    {
        InitializeComponent();

        _osxControlSpacer = this.Find<Rectangle>("OSXControlSpacer")!;
        _winControlSpacer = this.Find<Rectangle>("WinControlSpacer")!;
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        PropertyChanged += OnWindowPropertyChanged;

        _osxControlSpacer.IsVisible = _isOsx;
        _winControlSpacer.IsVisible = _isWin;
    }

    private void OnWindowPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property != Window.WindowStateProperty || e.NewValue is not WindowState state) return;

        Console.WriteLine($"Window state changed to {state}");
        if (state == WindowState.FullScreen)
        {
            if (_isOsx) _osxControlSpacer.IsVisible = false;
            if (_isWin) _winControlSpacer.IsVisible = false;
        }
        else
        {
            if (_isOsx) _osxControlSpacer.IsVisible = true;
            if (_isWin) _winControlSpacer.IsVisible = true;
        }
    }
}
