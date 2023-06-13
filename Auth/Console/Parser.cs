using System.Text.RegularExpressions;
using static Crayon.Output;

namespace Auth.Console;

public static class Parser
{
	private static readonly Dictionary<string, Func<string, string>> ColorCodes = new()
	{
		{"§0", Black},
		{"§1", Blue},
		{"§2", Green},
		{"§3", Cyan},
		{"§4", Red},
		{"§5", Magenta},
		{"§7", Yellow},
		{"§f", White},
		{"§b", Bold},
		{"§u", Underline},
		{"§r", null!}
	};

	private static readonly Regex UntilColorCode = new("^[^§]*");

	public static string ParseRichText(string text, Dictionary<string, string>? replacements = null)
	{
		text = replacements?.Aggregate(text, (current, replacement) =>
			current.Replace($"{{{replacement.Key}}}", replacement.Value)) ?? text;

		if (text.Length <= 1)
			return text;

		var colorCode = text[..2];
		ColorCodes.TryGetValue(colorCode, out var func);
		if (func != null)
		{
			return func(ParseRichText(text[2..]));
		}

		var match = UntilColorCode.Match(text);
		return match.Value + ParseRichText(text[match.Length..]);
	}
}