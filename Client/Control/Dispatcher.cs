using Avalonia.Controls;
using Client.Model;
using Client.View.Content;
using Client.View.Sidebars;
using static Avalonia.Threading.Dispatcher;

namespace Client.Control; 

public static class Dispatcher {
    public static void SetContent(AbstractContent? content) {
        UIThread.Post(() => {
            Global.Content.CurrentContent = content ?? new EmptyPanel();
        });
    }
    
    public static void SetSidebar(AbstractSidebar? sidebar) {
        UIThread.Post(() => {
            Global.Sidebar.CurrentSidebar = sidebar ?? new EmptySidebar();
            Global.Sidebar.IsOpened = sidebar != null;
        });
    }
}