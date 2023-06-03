namespace Client.View.Content;

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

	public override double? WindowHeight => 630.0;
}