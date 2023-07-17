using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Client.ViewModel;

namespace Client.View;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    public LoginWindowViewModel LoginWindowViewModel => (DataContext as LoginWindowViewModel)!;

    private void BackButton_OnClick(object? sender, RoutedEventArgs e)
    {
        LoginWindowViewModel.Back();
    }

    private void MenuBar_OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (OperatingSystem.IsWindows())
        {
            MenuBar.SetValue(Grid.ColumnProperty, 0);
        }
        else if (OperatingSystem.IsMacOS())
        {
            MenuBar.SetValue(Grid.ColumnProperty, 2);
        }
    }

    private void MenuBar_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}