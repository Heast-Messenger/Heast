using System;
using Avalonia.Layout;

namespace Client.ViewModel.Sidebars; 

public abstract class SidebarViewModelBase {
    protected SidebarViewModelBase(ContentViewModel contentViewModel) {
        _contentViewModel = contentViewModel;
    }

    protected SidebarViewModelBase() {
        throw new InvalidOperationException("This constructor is only for design-time purposes.");
    }
	
    private readonly ContentViewModel _contentViewModel;
    
    public void NavigateTo(Layoutable destination) {
        _contentViewModel.Content = destination;
    }
}