package heast.core.network;

public enum NetworkSide {
	SERVER, CLIENT;

	public NetworkSide opposite() {
		return this == SERVER ? CLIENT : SERVER;
	}
}
