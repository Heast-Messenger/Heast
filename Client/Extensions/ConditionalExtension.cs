using System;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;

namespace Client.Extensions;

public class ConditionalExtension : MarkupExtension
{
	public ConditionalExtension(CompiledBindingExtension binding)
	{
		Binding = binding;
	}

	public CompiledBindingExtension Binding { get; }

	public object True { get; set; } = null!;

	public object False { get; set; } = null!;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		{
			Binding.Mode = BindingMode.OneWay;
			Binding.Converter = new FuncValueConverter<bool, object>(
				value => value ? True : False);
		}

		// return Binding.ProvideValue(serviceProvider);
		return Binding;
	}
}