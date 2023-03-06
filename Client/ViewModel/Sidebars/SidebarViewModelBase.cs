using System;
using Client.Model;

namespace Client.ViewModel.Sidebars; 

public abstract class SidebarViewModelBase {
    protected SidebarViewModelBase(ContentViewModel contentViewModel) {
        _contentViewModel = contentViewModel;
    }

    protected SidebarViewModelBase() {
        throw new InvalidOperationException("This constructor is only for design-time purposes.");
    }
	
    private readonly ContentViewModel _contentViewModel;
    
    public void NavigateTo<T>(T item) where T : SidebarItemBase {
        _contentViewModel.Content = item.ContentBase;
    }
}