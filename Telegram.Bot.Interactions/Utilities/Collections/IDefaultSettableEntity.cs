namespace Telegram.Bot.Interactions.Utilities.Collections;

/// <summary>
/// Interface for any entities that can be marked as defaults.
/// </summary>
public interface IDefaultSettableEntity
{
    public bool Default { get; }
}