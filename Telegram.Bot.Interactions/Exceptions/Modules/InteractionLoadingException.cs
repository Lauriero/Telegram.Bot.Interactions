using Telegram.Bot.Interactions.Model;

namespace Telegram.Bot.Interactions.Exceptions.Modules;

public class InteractionLoadingException : Exception
{
    public IInteraction Interaction { get; }

    public InteractionLoadingException(IInteraction interaction, string message)
        : base($"Loading interaction with id {interaction.Id} failed: {message}")
    {
        Interaction = interaction;
    }
}