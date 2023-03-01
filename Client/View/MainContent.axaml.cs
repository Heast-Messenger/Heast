using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client.View; 

public partial class MainContent : UserControl {
    public MainContent() {
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}