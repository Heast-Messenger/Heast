namespace Client.View.Content;

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

	public override double? WindowHeight => 450;
}