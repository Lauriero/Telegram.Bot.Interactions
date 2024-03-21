using System.Runtime.CompilerServices;

namespace Telegram.Bot.Interactions.Utilities.Extensions;

public static class AsyncExtensions
{
    /// <summary>
    /// Casts the result type of the input task as if it were covariant
    /// </summary>
    /// <typeparam name="T">The original result type of the task</typeparam>
    /// <typeparam name="TResult">The covariant type to return</typeparam>
    /// <param name="task">The target task to cast</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async ValueTask<TResult> AsTask<T, TResult>(this ValueTask<T> task) 
        where T : TResult 
        where TResult : class
    {
        return await task;
    }
}