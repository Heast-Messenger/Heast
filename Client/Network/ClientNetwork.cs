using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Core.Network.Packets.C2S;
using Core.Network.Pipeline;

namespace Client.Network;

public static class ClientNetwork
{
	public static ClientConnection? Ctx { get; set; }
	public static ConcurrentQueue<Action> ActionQueue { get; } = new();

	[SuppressMessage("ReSharper", "FunctionNeverReturns")]
	public static void Initialize(string[] args)
	{
		Console.WriteLine("Initializing client network...");

		while (true)
		{
			if (ActionQueue.TryDequeue(out var action))
			{
				try
				{
					action();
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error while executing network action: {ex.Message}");
				}
			}

			// We don't wanna cook eggs with our CPU
			Thread.Sleep(0);
		}
	}

	public static void RunOnNetworkThread(Action action)
	{
		ActionQueue.Enqueue(action);
	}

	public static void Connect(string host, int port)
	{
		// ReSharper disable once AsyncVoidLambda
		RunOnNetworkThread(async () =>
		{
			Console.WriteLine($"Connecting to {host}:{port}...");
			Ctx = await ClientConnection.GetServerConnection(host, port);
			Ctx.Listener = new ClientLoginHandler(Ctx);
			await Ctx.Send(new HelloC2SPacket("Heast Client"));
		});
	}
}