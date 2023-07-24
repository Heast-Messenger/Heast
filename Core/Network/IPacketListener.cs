namespace Core.Network;

public interface IPacketListener
{
    public TaskCompletionSource TaskCompletionSource { get; }
}