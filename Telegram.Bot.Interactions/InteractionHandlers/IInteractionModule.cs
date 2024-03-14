using Telegram.Bot.Interactions.Model;

namespace Telegram.Bot.Interactions.InteractionHandlers;

public interface IInteractionModule
{
    /// <summary>
    /// This method serves to declare interactions that will be parsed automatically
    /// when this module is loaded into the <see cref="IInteractionService"/>.
    /// </summary>
    public IEnumerable<IInteraction> DeclareInteractions();
}