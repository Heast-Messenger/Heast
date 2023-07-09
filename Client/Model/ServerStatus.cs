using Client.Attributes;

namespace Client.Model;

public enum ServerStatus
{
	[StringValue("Pending")] Pending,
	[StringValue("Down")] Closed,
	[StringValue("Up")] Running,
	[StringValue("Slow")] Slow
}