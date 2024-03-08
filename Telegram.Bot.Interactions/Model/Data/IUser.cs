namespace Telegram.Bot.Interactions.Model.Data;

/// <summary>
/// 
/// </summary>
public interface IUser
{
    /// <summary>
    /// 
    /// </summary>
    public long Id { get; }
    
    /// <summary>
    /// If set, identifies the interaction that is currently going on.
    /// The next user message will be handled with the restrictions described
    /// in the proper <see cref="Interaction"/> with the same <see cref="Interaction.Id"/>.
    /// </summary>
    public int? CurrentInteractionId { get; }
}