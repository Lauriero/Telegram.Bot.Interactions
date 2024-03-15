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

    public string EntityName { get; }
    
    public Exception? LoadingException { get; }
    
    public TEntity? Entity { get; }
    
    public GenericLoadingResult(bool loaded, string entityName, Exception? loadingException, TEntity? entity)
    {
        Loaded           = loaded;
        Entity           = entity;
        EntityName       = entityName;
        LoadingException = loadingException;
    }

    public static GenericLoadingResult<TEntity> FromSuccess(string entityName, TEntity entity) =>
        new GenericLoadingResult<TEntity>(true, entityName, null, entity);
    
    public static GenericLoadingResult<TEntity> FromFailure(string entityName, Exception exception) =>
        new GenericLoadingResult<TEntity>(false, entityName, exception, null);
}