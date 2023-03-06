using Avalonia.Layout;
using Client.View.Content;

namespace Client.ViewModel; 

public class ContentViewModel : ViewModelBase {
    public ContentViewModel() {
        _content = new EmptyPanel();
    }
    
    private Layoutable _content;
    public Layoutable Content {
        get => _content;
        set => RaiseAndSetIfChanged(ref _content, value);
    }
}