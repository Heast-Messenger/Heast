using Avalonia.Layout;
using Client.View.Content;

namespace Client.ViewModel;

public class ContentViewModel : ViewModelBase
{
    private Layoutable _content;

    public ContentViewModel()
    {
        _content = new HomePanel();
    }

    public Layoutable Content
    {
        get => _content;
        set => RaiseAndSetIfChanged(ref _content, value);
    }
}