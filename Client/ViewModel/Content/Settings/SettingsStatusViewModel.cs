namespace Client.ViewModel.Content;

public class SettingsStatusViewModel : ContentViewModelBase
{
    private readonly MainWindowViewModel _mainWindowVm;

    public SettingsStatusViewModel(MainWindowViewModel mainWindowVm)
    {
        _mainWindowVm = mainWindowVm;
    }
}