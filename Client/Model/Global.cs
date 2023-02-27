using Client.ViewModel;

namespace Client.Model; 

public static class Global {
    public static SidebarViewModel Sidebar { get; set; } = null!;
    public static ContentViewModel Content { get; set; } = null!;
}