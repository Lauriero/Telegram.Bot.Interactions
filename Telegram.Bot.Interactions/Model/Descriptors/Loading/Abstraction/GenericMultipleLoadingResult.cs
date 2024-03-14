using System.Collections.ObjectModel;

namespace Telegram.Bot.Interactions.Model.Descriptors.Loading.Abstraction;

/// <summary>
/// Accumulates the results of loading entities of the same type.
/// If all the entities were loaded, <see cref="Loaded"/> is set to true. 
/// </summary>
public class GenericMultipleLoadingResult<TEntity> : ILoadingResult
    where TEntity : class
{
    public bool Loaded => Entities.Count == 0 || Entities.All(e => e.Loaded);
    
    public Exception? LoadingException { get; }

    /// <summary>
    /// List of the entities that should have been loaded.
    /// </summary>
    public IReadOnlyList<GenericLoadingResult<TEntity>> Entities { get; } 
        
    public GenericMultipleLoadingResult(IList<GenericLoadingResult<TEntity>> entities, 
        Exception? loadingException = null)
    {
        Entities         = new ReadOnlyCollection<GenericLoadingResult<TEntity>>(entities);
        LoadingException = loadingException;
    }
    
}
