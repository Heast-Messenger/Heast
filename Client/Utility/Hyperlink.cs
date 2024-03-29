using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Media;

namespace Client.Utility;

public class Hyperlink : TextBlock
{
	public static readonly StyledProperty<string> UrlProperty = AvaloniaProperty.Register<Run, string>(
		nameof(Text), defaultBindingMode: BindingMode.TwoWay);

	public string Url
	{
		get => GetValue(UrlProperty);
		set => SetValue(UrlProperty, value);
	}

	protected override void OnInitialized()
	{
		Foreground = Brushes.CornflowerBlue;
		Cursor = new(StandardCursorType.Hand);
		PointerPressed += (_, _) =>
		{
			Process.Start(new ProcessStartInfo(Url)
			{
				UseShellExecute = true
			});
		};
	}
}