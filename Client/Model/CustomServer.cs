using System;
using Avalonia.Media;

namespace Client.Model;

public class CustomServer
{
	public string? Name { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string? Description { get; set; }
	public IObservable<IImage>? Image { get; set; }
}