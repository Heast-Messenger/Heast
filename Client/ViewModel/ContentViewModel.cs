using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Threading;

namespace Client.ViewModel; 

public class ContentViewModel : ViewModelBase {
    public ContentViewModel() {
        _currentContent = new Panel();
        
        Dispatcher.UIThread.InvokeAsync(async () => {
            await Task.Delay(3000);
            CurrentContent = new TextBlock { Text = "Hello World!" };
        }, DispatcherPriority.Layout);
    }
    
    private Layoutable _currentContent;
    public Layoutable CurrentContent {
        get => _currentContent;
        set => RaiseAndSetIfChanged(ref _currentContent, value);
    }
}