namespace Core.Network;

public struct NetworkSide
{
    public override bool Equals(object? obj)
    {
        return obj is NetworkSide other && Equals(other);
    }

    private int InternalValue { get; set; }

    public static readonly NetworkSide Client = 0;
    public static readonly NetworkSide Server = 1;

    public static NetworkSide operator !(NetworkSide side)
    {
        return side == Client
            ? Server
            : Client;
    }

    public static implicit operator NetworkSide(int otherType)
    {
        return new NetworkSide
        {
            InternalValue = otherType
        };
    }

    public static bool operator ==(NetworkSide left, NetworkSide right)
    {
        return left.InternalValue == right.InternalValue;
    }

    public static bool operator !=(NetworkSide left, NetworkSide right)
    {
        return !(left == right);
    }

    private bool Equals(NetworkSide other)
    {
        return InternalValue == other.InternalValue;
    }

    public override int GetHashCode()
    {
        return InternalValue;
    }
}