using System.Collections.ObjectModel;

namespace Telegram.Bot.Interactions.Model.Descriptors.Loading.Abstraction;

public class MultipleLoadingResult<TEntity> : ILoadingResult
    where TEntity : ILoadingResult
{
    public bool Loaded => Entities?.All(e => e.Loaded) ?? false;
    public Exception? LoadingException { get; }
    public IReadOnlyList<TEntity>? Entities { get; }

    private MultipleLoadingResult(IList<TEntity>? entities = null, 
        Exception? exception = null)
    {
        LoadingException = exception;

        if (entities is not null) {
            Entities         = new ReadOnlyCollection<TEntity>(entities);
        }
    }

    public static MultipleLoadingResult<TEntity> FromSuccess(IList<TEntity> entities)
    {
        return new MultipleLoadingResult<TEntity>(entities);
    }

    public static MultipleLoadingResult<TEntity> FromFailure(Exception exception)
    {
        return new MultipleLoadingResult<TEntity>(exception: exception);
    }
}
