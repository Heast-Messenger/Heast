using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client.View.Sidebars; 

public partial class FriendsList : UserControl {
    public FriendsList() {
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}