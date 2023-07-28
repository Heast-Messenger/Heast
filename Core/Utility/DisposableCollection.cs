using System.Collections.ObjectModel;

namespace Core.Utility;

public class DisposableCollection : Collection<IDisposable>, IDisposable
{
    public DisposableCollection(IList<IDisposable> items)
        : base(items)
    {
    }

    public DisposableCollection()
    {
    }

    public void Dispose()
    {
        foreach (var item in this)
        {
            try
            {
                item.Dispose();
            }
            catch
            {
                // swallow
            }
        }
    }
}