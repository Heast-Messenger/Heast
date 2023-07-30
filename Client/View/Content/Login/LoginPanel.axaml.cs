namespace Client.View.Content.Login;

public partial class LoginPanel : LoginBase
{
    public LoginPanel()
    {
        InitializeComponent();
    }

    public override LoginBase Back => new LoginOptionsPanel
    {
        DataContext = DataContext
    };
}