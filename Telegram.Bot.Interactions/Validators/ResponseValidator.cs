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
    /// <inheritdoc cref="ValidateResponseAsync"/>
    public abstract ValueTask<bool> ValidateAsync(TResponse response);

    /// <summary>
    /// Contains a strongly type <see cref="Config"/> property.
    /// </summary>
    public IResponseModelConfig<TResponse>? ValidationConfig { get; set; }

    public IResponseModelConfig<IUserResponse>? Config
    {
        get => (IResponseModelConfig<IUserResponse>?)ValidationConfig;
        set => ValidationConfig = (IResponseModelConfig<TResponse>?)value;
    }

    public ValueTask<bool> ValidateResponseAsync(IUserResponse response)
    {
        return ValidateAsync((TResponse)response);
    }
}