package heast.core.network;

public enum NetworkSide {
	SERVER, CLIENT;

	public NetworkSide opposite() {
		return this == CLIENT ? SERVER : CLIENT;
	}
}
