namespace Core.Extensions;

public static class FlagsMapperExtension
{
    /// <summary>
    ///     Maps an enumeration of type TIn to the output (flags)-enum TResult and
    ///     ORs the values together.
    /// </summary>
    /// <param name="s">The enumerable to apply the operation on.</param>
    /// <param name="f">The transformation function to map a given type TIn to TResult</param>
    /// <typeparam name="TIn">The inner type of the enumerable.</typeparam>
    /// <typeparam name="TResult">The target flags-enum type.</typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentException">If the type TResult is not defined as being flags.</exception>
    public static TResult MapFlags<TIn, TResult>(this IEnumerable<TIn> s, Func<TIn, TResult> f)
    {
        var attribute = Attribute.GetCustomAttribute(typeof(TResult),
            typeof(FlagsAttribute));

        if (attribute is null)
        {
            throw new ArgumentException("Enum 'TResult' is not defined as a 'Flags'-Enum");
        }

        // Set 0 ulong as initial value, and 'or' the flags together.
        var result = s.Aggregate(seed: 0, (current, @in) => current | Convert.ToInt32(f(@in)));
        return (TResult)(object)result;
    }
}