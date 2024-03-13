using Telegram.Bot.Interactions.Model.Responses.Abstraction;

namespace Telegram.Bot.Interactions.Model.Context;

/// <summary>
/// Contains data of the context
/// in which user has responded to an interaction.
/// </summary>
public interface IInteractionContext<out TResponse>
    where TResponse : IUserResponse
{
    /// <summary>
    /// Service that handled the interaction.
    /// </summary>
    IInteractionService InteractionService { get; }

    /// <summary>
    /// Interaction to which the user has responded.
    /// </summary>
    IInteraction TargetInteraction { get; }

    /// <summary>
    /// One of the <see cref="IInteraction.AvailableResponses"/>
    /// that carries the <see cref="Response"/>.
    /// </summary>
    IResponseModel<TResponse> ResponseModel { get; }

    /// <summary>
    /// Contains the user's response.
    /// </summary>
    TResponse Response { get; }
}