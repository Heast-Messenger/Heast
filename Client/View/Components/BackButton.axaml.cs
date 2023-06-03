using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Client.View.Components;

public partial class BackButton : UserControl
{
	public static readonly RoutedEvent<RoutedEventArgs> ClickEvent =
		RoutedEvent.Register<BackButton, RoutedEventArgs>(nameof(Click), RoutingStrategies.Bubble);

	public BackButton()
	{
		InitializeComponent();
	}

	public event EventHandler<RoutedEventArgs>? Click
	{
		add => AddHandler(ClickEvent, value);
		remove => RemoveHandler(ClickEvent, value);
	}

	private void Button_OnClick(object? sender, RoutedEventArgs e)
	{
		RaiseEvent(new RoutedEventArgs(ClickEvent, sender));
	}
}