using Avalonia.Layout;
using Client.View.Content;

namespace Client.ViewModel;

public class ContentViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainWindowVm;
    private Layoutable _content;

    public ContentViewModel(MainWindowViewModel mainWindowVm)
    {
        _content = new HomePanel();
        _mainWindowVm = mainWindowVm;
    }

    public Layoutable Content
    {
        get => _content;
        set => RaiseAndSetIfChanged(ref _content, value);
    }
}