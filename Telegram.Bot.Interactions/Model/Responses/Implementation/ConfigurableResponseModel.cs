using System.Diagnostics.CodeAnalysis;

using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Validators;

namespace Telegram.Bot.Interactions.Model.Responses.Implementation;

/// <summary>
/// Response model, that uses <see cref="Config"/> to
/// set the verification parameters for the response.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public class ConfigurableResponseModel<TResponse> : IResponseModel<TResponse>
    where TResponse : IUserResponse
{
    /// <inheritdoc />
    public string Key { get; }

    /// <inheritdoc />
    [MemberNotNullWhen(true, nameof(Valid))]
    [MemberNotNullWhen(true, nameof(Response))]
    public bool Responded { get; internal set; }
    
    /// <inheritdoc />
    public bool? Valid { get; internal set; }

    /// <inheritdoc />
    public TResponse? Response { get; }
    
    /// <inheritdoc />
    public Type ResponseParserType { get; }

    /// <summary>
    /// Configures the response validation.
    /// </summary>
    public IResponseModelConfig<TResponse> Config { get; }
    
    /// <summary>
    /// Type of the validator that implements <see cref="IResponseValidator{TResponse}"/> interface
    /// and will be used to validate the response.
    /// </summary>
    public Type ValidatorType { get; }

    public ConfigurableResponseModel(string key, Type responseParserType,
        IResponseModelConfig<TResponse> config, Type validatorType)
    {
        // TODO: Validate validator type
        // TODO: Validate parser type

        Key = key;
        Config = config;
        Responded = false;
        ResponseParserType = responseParserType;
        ValidatorType = validatorType;
    }
}
