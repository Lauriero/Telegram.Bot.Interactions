using Telegram.Bot.Interactions.Model.Responses.Abstraction;

namespace Telegram.Bot.Interactions.Validators;

/// <summary>
/// Type-safe implementation of the response validator.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public abstract class ResponseValidator<TResponse> : IResponseValidator<TResponse>
    where TResponse : IUserResponse
{
    /// <inheritdoc cref="ValidateResponseAsync"/>
    public abstract ValueTask<bool> ValidateAsync(TResponse response);
    
    public ValueTask<bool> ValidateResponseAsync(IUserResponse response)
    {
        return ValidateAsync((TResponse)response);
    }
}