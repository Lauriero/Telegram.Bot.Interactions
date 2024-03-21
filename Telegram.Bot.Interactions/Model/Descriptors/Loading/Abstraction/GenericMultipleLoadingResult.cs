using System.Collections.ObjectModel;

namespace Telegram.Bot.Interactions.Model.Descriptors.Loading.Abstraction;

/// <summary>
/// Accumulates the results of loading entities of the same type.
/// If all the entities were loaded, <see cref="Loaded"/> is set to true. 
/// </summary>
public class GenericMultipleLoadingResult<TEntity> : ILoadingResult
    where TEntity : class
{
    public bool Loaded { get; }
    
    public Exception? LoadingException { get; }

    /// <summary>
    /// List of the entities that should have been loaded.
    /// </summary>
    public IReadOnlyList<GenericLoadingResult<TEntity>>? Entities { get; } 
        
    private GenericMultipleLoadingResult(bool loaded, IList<GenericLoadingResult<TEntity>>? entities = null, 
        Exception? loadingException = null)
    {
        Loaded           = loaded;
        LoadingException = loadingException;

        if (entities is not null) {
            Entities = new ReadOnlyCollection<GenericLoadingResult<TEntity>>(entities);
        }
    }

    public static GenericMultipleLoadingResult<TEntity> FromSuccess(
        IList<GenericLoadingResult<TEntity>> results)
    {
        return new GenericMultipleLoadingResult<TEntity>(true, results);
    }

    public static GenericMultipleLoadingResult<TEntity> FromFailure(Exception exception)
    {
        return new GenericMultipleLoadingResult<TEntity>(false, loadingException: exception);
    }
}
