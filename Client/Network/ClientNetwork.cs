using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Core.Network.Packets.C2S;
using Core.Network.Codecs;

namespace Client.Network;

public static class ClientNetwork
{
	public static ClientConnection? Ctx { get; set; }
	public static ConcurrentQueue<IJob> ActionQueue { get; } = new();

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
					action.Run();
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

	public static Task<TResult> RunAsync<TResult>(Func<TResult> function)
	{
		var job = new JobWithResult<TResult>(function);
		ActionQueue.Enqueue(job);
		return job.Task;
	}

	public static void Post(Action action)
	{
		var job = new Job(action);
		ActionQueue.Enqueue(job);
	}

	public static Task Connect(string host, int port)
	{
		return RunAsync(async () =>
		{
			Console.WriteLine($"Connecting to {host}:{port}...");
			Ctx = await ClientConnection.ServerConnect(host, port);
			Ctx.Listener = new ClientLoginHandler(Ctx);
			await Ctx.Send(new HelloC2SPacket("Heast Client"));
		});
	}
}