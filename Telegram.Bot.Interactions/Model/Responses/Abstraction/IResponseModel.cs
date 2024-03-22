using System.Diagnostics.CodeAnalysis;

using Telegram.Bot.Interactions.Validators;
using Telegram.Bot.Interactions.Validators.Configs;

namespace Telegram.Bot.Interactions.Model.Responses.Abstraction;

/// <summary>
/// Describes the possible response for a specific interaction.
/// If the response is received, contains the data of the response.
/// </summary>
/// <typeparam name="TResponse">
/// The response type data this response model represents.
/// </typeparam>
public interface IResponseModel<out TResponse>
    where TResponse : IUserResponse
{
    /// <summary>
    /// Is used to identify the response for the interaction.
    /// </summary>
    public string Key { get; }
    
    /// <summary>
    /// Determines whether the user responded to an interaction
    /// according to this response model.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Valid))]
    [MemberNotNullWhen(true, nameof(Response))]
    public bool Responded { get; }
    
    /// <summary>
    /// If set, determines whether that the user response was valid.
    /// Not set if the user hasn't responded.
    /// </summary>
    public bool? Valid { get; }

    /// <summary>
    /// Set if the user has responded to an interaction
    /// and contains data about his response in this case.
    /// </summary>
    public TResponse? Response { get; }

    public Type ResponseType { get; }

    /// <summary>
    /// Specifies the type of the parser that will be used to process the response.
    /// Is loaded when the response model is processed by the interaction service.
    /// </summary>
    public Type? ResponseParserType { get; set; }
    
    /// <summary>
    /// If set, determines the implementation type of the <see cref="ResponseValidator"/>
    /// and will be used to instantiate new instances of the validator during the loading,
    /// configuring it automatically with <see cref="Config"/>.
    /// If not set, the <see cref="ResponseValidator"/>, if provided, will be used instead to
    /// validate the response, and the <see cref="Config"/> will not be used to configure
    /// the validation process.
    /// Should be a type derived from <see cref="IResponseValidator{TResponse}"/>.
    /// </summary>
    public Type? ResponseValidatorType { get; }
    
    /// <summary>
    /// If set together with the <see cref="ResponseValidatorType"/>, will be injected
    /// into instantiated <see cref="ResponseValidator"/> during the loading process.
    /// </summary>
    public IResponseModelConfig<TResponse>? Config { get; }
    
    /// <summary>
    /// If set, will be used to validate the response.
    /// Doesn't need to be set, if <see cref="ResponseValidatorType"/> is set.
    /// </summary>
    public IResponseValidator<TResponse>? ResponseValidator { get; }
}