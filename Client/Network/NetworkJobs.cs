using System;
using System.Threading.Tasks;

namespace Client.Network;

public interface IJob
{
    void Run();
}

public class JobWithResult<TResult> : IJob
{
    private readonly Func<TResult> _function;
    private readonly TaskCompletionSource<TResult> _taskCompletionSource;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Job" /> class.
    /// </summary>
    /// <param name="function">The method to call.</param>
    public JobWithResult(Func<TResult> function)
    {
        _function = function;
        _taskCompletionSource = new TaskCompletionSource<TResult>();
    }

    public Task<TResult> Task => _taskCompletionSource.Task;

    public void Run()
    {
        try
        {
            var result = _function();
            _taskCompletionSource.SetResult(result);
        }
        catch (Exception e)
        {
            _taskCompletionSource.SetException(e);
        }
    }
}

public class Job : IJob
{
    /// <summary>
    ///     The method to call.
    /// </summary>
    private readonly Action _action;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Job" /> class.
    /// </summary>
    /// <param name="action">The method to call.</param>
    public Job(Action action)
    {
        _action = action;
    }

    public void Run()
    {
        try
        {
            _action();
        }
        catch (Exception)
        {
            // ignored
        }
    }
}