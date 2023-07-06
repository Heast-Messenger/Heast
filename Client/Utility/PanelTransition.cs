using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Media;
using Avalonia.Styling;

namespace Client.Utility;

public class PanelTransition : IPageTransition
{
	private readonly Animation _in;
	private readonly Animation _out;

	public PanelTransition()
		: this(TimeSpan.Zero)
	{
	}

	public PanelTransition(TimeSpan duration)
	{
		_out = new Animation
		{
			FillMode = FillMode.Forward,
			Easing = new BounceEaseOut(),
			Children =
			{
				new KeyFrame
				{
					Cue = new Cue(0d),
					Setters =
					{
						new Setter { Property = ScaleTransform.ScaleYProperty, Value = 0d }
					}
				},
				new KeyFrame
				{
					Cue = new Cue(1d),
					Setters =
					{
						new Setter { Property = ScaleTransform.ScaleYProperty, Value = 1d }
					}
				}
			}
		};

		_in = new Animation
		{
			FillMode = FillMode.Forward,
			Easing = new BounceEaseOut(),
			Children =
			{
				new KeyFrame
				{
					Cue = new Cue(0d),
					Setters =
					{
						new Setter { Property = ScaleTransform.ScaleYProperty, Value = 1d }
					}
				},
				new KeyFrame
				{
					Cue = new Cue(1d),
					Setters =
					{
						new Setter { Property = ScaleTransform.ScaleYProperty, Value = 0d }
					}
				}
			}
		};

		Duration = duration;
	}

	public TimeSpan Duration
	{
		get => _out.Duration;
		set => _out.Duration = _in.Duration = value;
	}

	public async Task Start(
		Visual? from,
		Visual? to,
		bool forward,
		CancellationToken cancellationToken
	)
	{
		if (cancellationToken.IsCancellationRequested)
		{
			return;
		}

		var tasks = new List<Task>();

		if (from != null)
		{
			tasks.Add(_out.RunAsync(from, cancellationToken));
			Console.WriteLine($"From: {from.GetType()}");
		}

		if (to != null)
		{
			to.IsVisible = true;
			tasks.Add(_in.RunAsync(to, cancellationToken));
			Console.WriteLine($"To: {to.GetType()}");
		}

		await Task.WhenAll(tasks);

		if (from != null && !cancellationToken.IsCancellationRequested)
		{
			from.IsVisible = false;
		}
	}
}