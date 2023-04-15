namespace Client.ViewModel.Content;

public class SettingsAppearanceViewModel : ContentViewModelBase
{
    private readonly MainWindowViewModel _mainWindowVm;

    public SettingsAppearanceViewModel(MainWindowViewModel mainWindowVm)
    {
        _mainWindowVm = mainWindowVm;
    }
}