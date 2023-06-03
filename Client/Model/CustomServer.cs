using Avalonia.Media;

namespace Client.Model;

public class CustomServer
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string? Description { get; set; }
	public IImage? Image { get; set; }
}