using System.Diagnostics.CodeAnalysis;

namespace Telegram.Bot.Interactions.Model.Descriptors.Loading.Abstraction;

/// <summary>
/// Wraps the entity of the type <typeparamref name="TEntity"/>
/// aroung the loading model, providing the loading metadata
/// in accordance with the general <see cref="ILoadingResult"/>.
/// </summary>
public class GenericLoadingResult<TEntity> : ILoadingResult
    where TEntity : class
{
    [MemberNotNullWhen(true, nameof(Entity))]
    public bool Loaded { get; }
    
    public Exception? LoadingException { get; }
    
    public TEntity? Entity { get; }
    
    public GenericLoadingResult(bool loaded, Exception? loadingException, TEntity? entity)
    {
        Loaded           = loaded;
        Entity           = entity;
        LoadingException = loadingException;
    }

    public static GenericLoadingResult<TEntity> FromSuccess(TEntity entity) =>
        new GenericLoadingResult<TEntity>(true, null, entity);
    
    public static GenericLoadingResult<TEntity> FromFailure(Exception exception) =>
        new GenericLoadingResult<TEntity>(true, exception, null);
}