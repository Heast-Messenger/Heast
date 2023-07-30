using Avalonia.Controls;
using Client.ViewModel;

namespace Client.View.Controls;

public class ModalBase : UserControl
{
    private ModalViewModel ModalViewModel => (DataContext as ModalViewModel)!;

    public void Close()
    {
        ModalViewModel.Close();
    }
}