using Avalonia;

namespace Client.Converter;

public class SmoothDamp
{
	public static double Read(double from, double to, ref double vel, double st, double dt)
	{
		var omega = 2.0 / st;
		var x = omega * dt;
		var exp = 1.0 / (1.0 + x + 0.48 * x * x + 0.235 * x * x * x);
		var change = from - to;
		var temp = (vel + omega * change) * dt;
		vel = (vel - omega * temp) * exp;
		return to + (change + temp) * exp;
	}

	public static Size Read2d(Size from, Size to, ref Size vel, double st, double dt)
	{
		var omega = 2.0 / st;
		var x = omega * dt;
		var exp = 1.0 / (1.0 + x + 0.48 * x * x + 0.235 * x * x * x);
		var change = from - to;
		var temp = (vel + change * omega) * dt;
		vel = (vel - temp * omega) * exp;
		return to + (change + temp) * exp;
	}
}