using Core.Network.Listeners;

namespace Core.Network.Packets.C2S; 

public class ErrorC2SPacket : IPacket<IServerLoginListener> {
	
	public Error Error { get; }
    
	public ErrorC2SPacket(Error error) {
		Error = error;
	}
    
	public ErrorC2SPacket(PacketBuf buf) {
		Error = buf.ReadEnum<Error>();
	}
	
	public void Write(PacketBuf buf) {
		buf.WriteEnum(Error);
	}

	public void Apply(IServerLoginListener listener) {
		listener.OnError(this);
	}
}