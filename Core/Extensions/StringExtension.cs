using System.Text;

namespace Core.Extensions;

public static class StringExtension
{
	public static string Repeat(this string str, int count)
	{
		var sb = new StringBuilder();
		for (var i = 0; i < count; i++)
		{
			sb.Append(str);
		}

		return sb.ToString();
	}
}