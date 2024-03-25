using Telegram.Bot.Interactions.Model.Responses.Abstraction;

namespace Telegram.Bot.Interactions.Model;

/// <summary>
/// Describes the interaction between the bot and the user.
/// Allows to configure the responses, and present them in a structured manner.
/// The interaction is presented as a identifiable list of the responses, that can be received by the bot,
/// when the interaction is in process.
/// To set the interaction response the property <see cref="IUser.CurrentInteractionId"/> is used.
/// </summary>
public interface IInteraction
{
    /// <summary>
    /// Identifies the interaction.
    /// </summary>
    uint Id { get; }

    /// <summary>
    /// List of the responses that user can give or gave to an interaction.
    /// </summary>
    IReadOnlyList<IResponseModel> AvailableResponses { get; }
}