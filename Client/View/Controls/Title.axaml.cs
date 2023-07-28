using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client.View.Content.Login.Templates;

public partial class Title : UserControl
{
    public Title()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}