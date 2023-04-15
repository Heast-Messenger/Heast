using System;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Client.View;

public class NavigationEasing : Easing
{
    public override double Ease(double t)
    {
        return (1 - t) * Math.Sin(t * Math.PI * 3) / 36.0;
    }
}

public partial class Navigation : UserControl
{
    public Navigation()
    {
        InitializeComponent();

        PointerEnteredEvent.AddClassHandler<Button>(Button_OnHover);
    }

    private static void Button_OnHover(Button sender, RoutedEventArgs args)
    {
        if (sender.Classes.Contains("NavButton")) sender.Classes.Add("Hover");
    }
}