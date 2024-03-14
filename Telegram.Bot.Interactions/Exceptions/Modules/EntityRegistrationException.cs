namespace Telegram.Bot.Interactions.Exceptions.Modules;

public class EntityRegistrationException<TEntity> : Exception
{
    public TEntity Entity { get; }

    public EntityRegistrationException(TEntity entity, string message) 
        : base($"Entity registration failed: {message}")
    {
        Entity = entity;
    }
}