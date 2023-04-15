namespace Client.ViewModel.Content;

public class SettingsNotificationsViewModel : ContentViewModelBase
{
    private readonly MainWindowViewModel _mainWindowVm;

    public SettingsNotificationsViewModel(MainWindowViewModel mainWindowVm)
    {
        _mainWindowVm = mainWindowVm;
    }
}