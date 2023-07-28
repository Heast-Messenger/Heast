namespace Core.Extensions;

public static class TaskExtension
{
    public static async Task<TV> Then<T, TV>(this Task<T> task, Func<T, TV> then)
    {
        var result = await task;
        return then(result);
    }
}