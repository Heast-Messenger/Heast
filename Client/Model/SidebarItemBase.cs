using Client.View.Content;

namespace Client.Model; 

public abstract class SidebarItemBase {
    protected SidebarItemBase(ContentBase contentBase) {
        this.ContentBase = contentBase;
    }
    
    public ContentBase ContentBase { get; set; }
}