using System;
using Avalonia.Layout;

namespace Client.ViewModel.Sidebars; 

public abstract class SidebarViewModelBase {
    protected SidebarViewModelBase(ContentViewModel contentViewModel) {
        ContentViewModel = contentViewModel;
    }

    protected SidebarViewModelBase() {
        throw new InvalidOperationException("This constructor is only for design-time purposes.");
    }
	
    public readonly ContentViewModel ContentViewModel;
}