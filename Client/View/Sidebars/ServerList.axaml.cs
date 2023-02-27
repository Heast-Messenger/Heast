using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client.View.Sidebars; 

public partial class ServerList : UserControl {
    public ServerList() {
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}