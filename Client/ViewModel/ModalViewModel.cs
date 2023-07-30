using Client.View.Content.Modals;
using Client.View.Controls;

namespace Client.ViewModel;

public class ModalViewModel : ViewModelBase
{
    private ModalBase? _modal;

    public ModalBase? Modal
    {
        get
        {
            if (_modal is EmptyModal)
            {
                return null;
            }

            return _modal;
        }
        set
        {
            if (value is null)
            {
                RaiseAndSetIfChanged(ref _modal, new EmptyModal());
                return;
            }

            RaiseAndSetIfChanged(ref _modal, value);
        }
    }

    public void Close()
    {
        Modal = null;
    }
}