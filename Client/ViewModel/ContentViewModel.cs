using Avalonia.Layout;
using Client.View.Content;

namespace Client.ViewModel; 

public class ContentViewModel : ViewModelBase {
    public ContentViewModel() {
        _currentContent = new EmptyPanel();
    }
    
    private Layoutable _currentContent;
    public Layoutable CurrentContent {
        get => _currentContent;
        set => RaiseAndSetIfChanged(ref _currentContent, value);
    }
}