using System.Text.RegularExpressions;

namespace Core.Utility;

public static class Validation
{
	private static readonly Regex Domain = new(@"^([a-z0-9]+(-[a-z0-9]+)*\.)+[a-z]{2,}$");
	private static readonly Regex Ipv4 = new(@"^([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3})$");
	private static readonly Regex Localhost = new(@"^localhost$");

	public static bool IsIpv4(string ip)
	{
		return Ipv4.IsMatch(ip);
	}

	public static bool IsLocalhost(string ip)
	{
		return Localhost.IsMatch(ip);
	}

	public static bool IsDomain(string domain)
	{
		return Domain.IsMatch(domain);
	}

	public static bool IsHost(string host)
	{
		return IsIpv4(host) || IsDomain(host);
	}

	public static bool IsPort(int port)
	{
		return port <= 65535;
	}

	public static void Split(string s, out string? host, out int? port)
	{
		host = null;
		port = null;
		var parts = s.Split(":");
		if (parts.Length == 1)
		{
			if (int.TryParse(parts[0], out var value) && IsPort(value))
			{
				port = value;
			}
			else
			{
				host = parts[0];
			}
		}

		if (parts.Length > 1)
		{
			host = parts[0];
			if (int.TryParse(parts[1], out var value) && IsPort(value))
			{
				port = value;
			}
		}
	}

	public static bool Validate(string host, int port, out bool localhost, out bool domain, out bool ipv4)
	{
		localhost = IsLocalhost(host);
		domain = IsDomain(host);
		ipv4 = IsIpv4(host);
		return (localhost || domain || ipv4) && IsPort(port);
	}
}