using System;

namespace Client.ViewModel.Sidebars;

public abstract class SidebarViewModelBase
{
    public readonly ContentViewModel ContentViewModel;

    protected SidebarViewModelBase(ContentViewModel contentViewModel)
    {
        ContentViewModel = contentViewModel;
    }

    protected SidebarViewModelBase()
    {
        throw new InvalidOperationException("This constructor is only for design-time purposes.");
    }
}