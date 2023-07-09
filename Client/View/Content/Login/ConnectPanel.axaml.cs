using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Client.Model;

namespace Client.View.Content.Login;

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

	private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		if (sender is Control { DataContext: ConnectionStep step })
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = step.Helplink,
				UseShellExecute = true
			});
		}
	}
}