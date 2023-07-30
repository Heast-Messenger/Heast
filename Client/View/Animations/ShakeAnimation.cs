using System;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Media;
using Avalonia.Styling;

namespace Client.View;

public static partial class Animations
{
    public static Animation Shake { get; } = new()
    {
        Easing = Easing.Parse("0.36,0.07,0.19,0.97"),
        FillMode = FillMode.Both,
        Duration = TimeSpan.FromSeconds(0.80),
        Children =
        {
            new KeyFrame { Cue = new Cue(0.1), Setters = { new Setter(TranslateTransform.XProperty, value: -2.0) } },
            new KeyFrame { Cue = new Cue(0.2), Setters = { new Setter(TranslateTransform.XProperty, value: +4.0) } },
            new KeyFrame { Cue = new Cue(0.3), Setters = { new Setter(TranslateTransform.XProperty, value: -8.0) } },
            new KeyFrame { Cue = new Cue(0.4), Setters = { new Setter(TranslateTransform.XProperty, value: +8.0) } },
            new KeyFrame { Cue = new Cue(0.5), Setters = { new Setter(TranslateTransform.XProperty, value: -8.0) } },
            new KeyFrame { Cue = new Cue(0.6), Setters = { new Setter(TranslateTransform.XProperty, value: +8.0) } },
            new KeyFrame { Cue = new Cue(0.7), Setters = { new Setter(TranslateTransform.XProperty, value: -8.0) } },
            new KeyFrame { Cue = new Cue(0.8), Setters = { new Setter(TranslateTransform.XProperty, value: +4.0) } },
            new KeyFrame { Cue = new Cue(0.9), Setters = { new Setter(TranslateTransform.XProperty, value: -2.0) } }
        }
    };
}