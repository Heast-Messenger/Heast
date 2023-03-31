using Avalonia.Interactivity;
using Client.Model;
using Client.View.Components;

namespace Client.View.Sidebars; 

public partial class SettingsSidebar : SidebarBase {
    public SettingsSidebar() {
        InitializeComponent();
    }

    private void OnSidebarItemClick(object? sender, RoutedEventArgs e) {
        // The sender is the SidebarButton in this case.
        // Above the sender lies the ContentPresenter whose
        //  datacontext is the SettingsSidebarItem to which we'll navigate.
        // Why? Idk. I just know that it is the way it is. =)
        base.OnSidebarItemClick<SettingsSidebarItem>(
            (sender as SidebarButton)!.Parent!, e);
    }
}