using System;
using Avalonia.Controls;
using Avalonia.Styling;

namespace Client.View.Components;

public class RadioButton : Avalonia.Controls.RadioButton, IStyleable
{
    public Type StyleKey => typeof(Button);
}
