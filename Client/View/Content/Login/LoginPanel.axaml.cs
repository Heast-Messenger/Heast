using Avalonia;

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

	public override Size? WindowSize => new Size(400.0, 690.0);
}