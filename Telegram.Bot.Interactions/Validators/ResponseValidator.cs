using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Validators.Configs;

namespace Telegram.Bot.Interactions.Validators;

/// <summary>
/// Type-safe implementation of the response validator.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public abstract class ResponseValidator<TResponse> : IResponseValidator<TResponse>
    where TResponse : IUserResponse
{
    private IResponseModelConfig<TResponse>? _config;

    /// <inheritdoc cref="ValidateResponseAsync"/>
    public abstract ValueTask<bool> ValidateAsync(TResponse response);

    public IResponseModelConfig<TResponse>? Config => _config;

    public void SetConfig(object config)
    {
        // TODO: Add TParams validation
        _config = (IResponseModelConfig<TResponse>)config;
    }

    public ValueTask<bool> ValidateResponseAsync(IUserResponse response)
    {
        return ValidateAsync((TResponse)response);
    }
}