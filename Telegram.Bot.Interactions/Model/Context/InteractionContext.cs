using Telegram.Bot.Interactions.Model.Responses.Abstraction;

namespace Telegram.Bot.Interactions.Model.Context;

/// <inheritdoc />
public class InteractionContext<TResponse> : IInteractionContext<TResponse>
    where TResponse : IUserResponse
{
    /// <inheritdoc />
    public IInteractionService InteractionService { get; }
    
    /// <inheritdoc />
    public IInteraction TargetInteraction { get; }
    
    /// <inheritdoc />
    public string ResponseKey { get; }

    /// <inheritdoc />
    public TResponse Response { get; }
    
    public InteractionContext(IInteractionService interactionService, 
        IInteraction targetInteraction, string responseKey,
        TResponse response)
    {
        Response           = response;
        ResponseKey        = responseKey;
        TargetInteraction  = targetInteraction;
        InteractionService = interactionService;
    }
}