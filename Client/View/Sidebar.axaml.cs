using System;
using Avalonia.Animation.Easings;
using Avalonia.Controls;

namespace Client.View;

// public class SidebarEasing : Easing
// {
// 	private const double C1 = 1.70158;
// 	private const double C2 = C1 * 1.525;
//
// 	public override double Ease(double t)
// 	{
// 		if (t < 0.5)
// 		{
// 			return Math.Pow(2 * t, 2) * ((C2 + 1) * 2 * t - C2) / 2;
// 		}
//
// 		return (Math.Pow(2 * t - 2, 2) * ((C2 + 1) * (2 * t - 2) + C2) + 2) / 2;
// 	}
// }

public class SidebarEasing : Easing
{
	public override double Ease(double t)
	{
		return -(Math.Cos(Math.PI * t) - 1) / 2;
	}
}

public partial class Sidebar : UserControl
{
	public Sidebar()
	{
		InitializeComponent();
	}
}