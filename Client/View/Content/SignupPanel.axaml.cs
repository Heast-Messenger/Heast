using Avalonia;

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

	public override Size? WindowSize => new Size(400.0, 630.0);
}