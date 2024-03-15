namespace Telegram.Bot.Interactions.Model.Descriptors;

/// <summary>
/// Contains interaction metadata alongside the interaction itself. 
/// </summary>
public class InteractionInfo
{
    /// <summary>
    /// Reference to an interaction.
    /// </summary>
    public IInteraction Interaction;

    /// <summary>
    /// Descriptor of the method that will handle this interaction.
    /// </summary>
    public InteractionHandlerInfo? HandlerInfo;

    public InteractionInfo(IInteraction interaction, InteractionHandlerInfo? handlerInfo)
    {
        HandlerInfo     = handlerInfo;
        Interaction = interaction;
    }
}