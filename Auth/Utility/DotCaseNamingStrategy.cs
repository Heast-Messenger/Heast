using System.Text;

// ReSharper disable once CheckNamespace
namespace Newtonsoft.Json.Serialization;

public class DotCaseNamingStrategy : NamingStrategy
{
	protected override string ResolvePropertyName(string name)
	{
		return name.ToDotCase();
	}
}

public static class StringExtensions
{
	public static string ToDotCase(this string input)
	{
		if (string.IsNullOrEmpty(input))
			return input;

		var builder = new StringBuilder();
		builder.Append(char.ToLowerInvariant(input[0]));
		for (var i = 1; i < input.Length; i++)
		{
			var c = input[i];
			if (char.IsUpper(c))
			{
				builder.Append('.');
				builder.Append(char.ToLowerInvariant(c));
			}
			else
			{
				builder.Append(c);
			}
		}

		return builder.ToString();
	}
}