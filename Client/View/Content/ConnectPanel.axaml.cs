using Avalonia;

namespace Client.View.Content;

public partial class ConnectPanel : LoginBase
{
	public ConnectPanel()
	{
		InitializeComponent();
	}

	public override LoginBase Back => new CustomServerPanel
	{
		DataContext = DataContext
	};

	public override Size? WindowSize => new(400.0, 500.0);
}