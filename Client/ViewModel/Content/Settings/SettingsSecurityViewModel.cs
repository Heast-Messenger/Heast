namespace Client.ViewModel.Content;

public class SettingsSecurityViewModel : ContentViewModelBase
{
    private readonly MainWindowViewModel _mainWindowVm;

    public SettingsSecurityViewModel(MainWindowViewModel mainWindowVm)
    {
        _mainWindowVm = mainWindowVm;
    }
}