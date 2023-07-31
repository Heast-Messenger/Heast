namespace Core.Extensions;

public static class TaskExtension
{
    public static async Task<TV> Then<T, TV>(this Task<T> task, Func<T, TV> then)
    {
        var result = await task;
        return then(result);
    }

    public static async void Run<T>(this Task<T> task, Action<T> run)
    {
        var result = await task;
        run(result);
    }
}