using Client.View.Content;
using Client.View.Content.Modals;

namespace Client.ViewModel;

public class ModalViewModel : ViewModelBase
{
    private ModalBase? _modal = new EmailVerificationModal(null!);

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

    // TODO: maybe modal queuing?
}