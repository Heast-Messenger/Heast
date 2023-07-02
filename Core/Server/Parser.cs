using System.Text;
using System.Text.RegularExpressions;
using static Crayon.Output;

namespace Core.Server;

public static class Parser
{
	private static readonly Dictionary<string, Func<string, string>> Codes = new()
	{
		{ "§0", Black },
		{ "§1", Blue },
		{ "§2", Green },
		{ "§3", Cyan },
		{ "§4", Red },
		{ "§5", Magenta },
		{ "§7", Yellow },
		{ "§f", White },
		{ "§b", Bold },
		{ "§u", Underline },
		{ "§d", Dim }
	};

	private static readonly Regex UntilCode = new("^[^§]*");

	public static string ParseRichText(string text, Dictionary<string, object>? replacements = null)
	{
		text = replacements?.Aggregate(text, (current, replacement) =>
			       current.Replace($"{{{replacement.Key}}}", replacement.Value.ToString())) ??
		       text;

		var final = new StringBuilder();
		var segments = text.Split("§r");
		foreach (var segment in segments)
		{
			var parsed = ParseRichTextInternal(segment);
			final.Append(parsed);
		}

		return final.ToString();
	}

	private static string ParseRichTextInternal(string text)
	{
		if (text.Length <= 1)
			return text;

		var code = text[..2];
		Codes.TryGetValue(code, out var func);
		if (func != null)
		{
			var result = text[2..];
			return func(ParseRichTextInternal(result));
		}

		{
			var match = UntilCode.Match(text);
			var result = match.Value;
			var rest = text[match.Length..];
			return result + ParseRichTextInternal(rest);
		}
	}
}