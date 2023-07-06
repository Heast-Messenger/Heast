using System;
using Avalonia.Controls;

namespace Client.View.Controls;

public class RadioButton : Avalonia.Controls.RadioButton
{
	protected override Type StyleKeyOverride => typeof(Button);
}