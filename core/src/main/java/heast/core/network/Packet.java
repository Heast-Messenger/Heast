package heast.core.network;

public interface Packet<T extends PacketListener> {

    void write(PacketBuf buf);
    void apply(T listener);
}