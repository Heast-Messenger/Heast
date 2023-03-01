using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client.View.Sidebars; 

public partial class EmptySidebar : AbstractSidebar {
    public EmptySidebar() {
        InitializeComponent();
    }
    
    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}