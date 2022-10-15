package heast.client;

import heast.client.gui.ClientGui;
import heast.client.network.ClientNetwork;

public final class Client {
	private final String host;
	private final int port;

	public static void main(String... args) {
		String host = args.length > 0
			? args[0]
			: "localhost";

		int port = args.length > 1
			? Integer.parseInt(args[1])
			: 8080;

		Client client= new Client(host, port);
		client.start();
	}

	public Client(String host, int port) {
		this.host = host;
		this.port = port;
	}

	private void start() {
		ClientGui.INSTANCE.initialize();
		ClientNetwork.initialize(host, port);
	}
}
