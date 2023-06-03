using System;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Client.View.Components;

namespace Client.Extensions;

public class SimpleTextExtension : MarkupExtension
{
	public SimpleTextExtension(string text)
	{
		Text = text;
	}

	public string Text { get; }

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return new TextBlock
		{
			Classes = Classes.Parse("Bold"),
			[!TextBlock.ForegroundProperty] = new Binding
			{
				Path = "Foreground",
				RelativeSource = new RelativeSource
				{
					AncestorType = typeof(IconText)
				}
			},
			Text = Text
		};
	}
}