namespace Core.Network;

public abstract class AbstractPacket
{
    public Guid Guid { get; set; }
    public ErrorCodes Errors { get; set; }
    public abstract void Write(PacketBuf buf);
    public abstract void Apply(IPacketListener listener);

    public bool HasErrors()
    {
        return (int)Errors != (int)ErrorCodes.None;
    }

    public string GetErrors()
    {
        return Enum
            .GetValues(typeof(ErrorCodes)).Cast<ErrorCodes>()
            .Where(a => a != ErrorCodes.None)
            .Where(a => (Errors & a) == a)
            .Select(a => a.ToString())
            .Aggregate((current, next) => $"{current}, {next}");
    }
}