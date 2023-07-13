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

	private CompiledBindingExtension Binding { get; }

	public object True { get; set; } = null!;

	public object False { get; set; } = null!;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		Binding.Mode = BindingMode.OneWay;
		if (Binding.ConverterParameter is bool or null)
		{
			Binding.Converter = new FuncValueConverter<bool, object>(
				value => value ? True : False);
		}

		if (Binding.ConverterParameter != null)
		{
			Binding.Converter = new FuncValueConverter<object, object>(
				value => value != null ? True : False);
		}

		return Binding;
	}
}