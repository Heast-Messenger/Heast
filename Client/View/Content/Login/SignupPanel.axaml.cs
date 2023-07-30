namespace Client.View.Content.Login;

public partial class SignupPanel : LoginBase
{
    public SignupPanel()
    {
        InitializeComponent();
    }

    public override LoginBase Back => new LoginOptionsPanel
    {
        DataContext = DataContext
    };
}