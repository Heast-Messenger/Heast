using System.Text.RegularExpressions;

namespace Core.Utility;

public static class Validation
{
	private static readonly Regex Domain = new(@"^([a-z0-9]+(-[a-z0-9]+)*\.)+[a-z]{2,}(:\d{0,5})?$");
	private static readonly Regex Ipv4 = new(@"^([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3})(:[0-9]{1,5})?$");
	private static readonly Regex Localhost = new(@"^localhost(:[0-9]{1,5})?$");

	public static bool IsIpv4(string ip)
	{
		return Ipv4.IsMatch(ip) || Localhost.IsMatch(ip);
	}

	public static bool IsDomain(string domain)
	{
		return Domain.IsMatch(domain);
	}
}