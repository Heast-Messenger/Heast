namespace Client.View.Content;

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

	public override double? WindowHeight => 630.0;
}