namespace Core.Network;

[Flags]
public enum Capabilities
{
	None = 0,

	Ssl = 1 << 0
	// <id> = 1 << 1,
	// <id> = 1 << 2,
	// <id> = 1 << 3,
}