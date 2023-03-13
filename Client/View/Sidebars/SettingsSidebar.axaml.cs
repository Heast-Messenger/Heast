using Avalonia.Interactivity;
using Client.Model;
using SkiaSharp;

namespace Client.View.Sidebars; 

public partial class SettingsSidebar : SidebarBase {
    public SettingsSidebar() {
        InitializeComponent();
    }

    private void OnSidebarItemClick(object? sender, RoutedEventArgs e) {
        base.OnSidebarItemClick<SettingsSidebarItem>(sender, e);
    }
}