using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Data;
using Avalonia.Threading;

namespace Client.Utility;

public class Typewriter : TextBlock
{
	public static readonly StyledProperty<TimeSpan> IntervalProperty = AvaloniaProperty.Register<Run, TimeSpan>(
		nameof(Interval), defaultBindingMode: BindingMode.TwoWay);

	private string _text = string.Empty;
	private DispatcherTimer? _timer;

	public TimeSpan Interval
	{
		get => GetValue(IntervalProperty);
		set => SetValue(IntervalProperty, value);
	}

	protected override void OnInitialized()
	{
		_text = Text!;
		_timer = new DispatcherTimer(Interval, DispatcherPriority.Normal, Callback);
		Text = string.Empty;
		InvalidateTextLayout();
	}

	private void Callback(object? sender, EventArgs e)
	{
		if (Text == null)
		{
			return;
		}

		var i = Text.Length + 1;
		Text = i > _text.Length
			? string.Empty
			: _text[..i];

		InvalidateTextLayout();
	}
}