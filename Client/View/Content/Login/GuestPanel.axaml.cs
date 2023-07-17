using Avalonia;

namespace Client.View.Content.Login;

public partial class GuestPanel : LoginBase
{
    public GuestPanel()
    {
        InitializeComponent();
    }

    public override LoginBase Back => new LoginOptionsPanel
    {
        DataContext = DataContext
    };

    public override Size? WindowSize => new Size(400.0, 510.0);
}