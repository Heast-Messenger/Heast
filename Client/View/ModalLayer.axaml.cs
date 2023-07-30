using Avalonia.Controls;
using Avalonia.Input;
using Client.ViewModel;

namespace Client.View;

public partial class ModalLayer : UserControl
{
    public ModalLayer()
    {
        InitializeComponent();
        KeyDownEvent.AddClassHandler<TopLevel>(OnKeyDown, handledEventsToo: true);
    }

    private ModalViewModel ModalViewModel => (DataContext as ModalViewModel)!;

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            ModalViewModel.Close();
        }
    }
}