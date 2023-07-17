namespace Core.Network;

[Flags]
public enum Capabilities
{
    None = 0,

    Ssl = 1 << 0,
    Signup = 1 << 1,
    Login = 1 << 2,
    Guest = 1 << 3
}