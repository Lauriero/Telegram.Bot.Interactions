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
    public IResponseModel<TResponse> ResponseModel { get; }
   
    /// <inheritdoc />
    public TResponse Response { get; }
    
    public InteractionContext(IInteractionService interactionService, 
        IInteraction targetInteraction, IResponseModel<TResponse> responseModel,
        TResponse response)
    {
        Response           = response;
        ResponseModel      = responseModel;
        TargetInteraction  = targetInteraction;
        InteractionService = interactionService;
    }
}