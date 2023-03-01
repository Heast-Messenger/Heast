using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client.View.Sidebars; 

public partial class FriendList : AbstractSidebar {
    public FriendList() {
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}