using System.Diagnostics.CodeAnalysis;

using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Validators;
using Telegram.Bot.Interactions.Validators.Configs;

namespace Telegram.Bot.Interactions.Model.Responses.Implementation;

/// <inheritdoc />
public class BasicResponseModel<TResponse> : IResponseModel
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
    public object? Response
    {
        get => TypedResponse;
        set => TypedResponse = (TResponse?)value;
    }
    
    public TResponse? TypedResponse { private get; set; }

    public Type ResponseType { get; }

    /// <inheritdoc />
    public Type? ResponseParserType { get; set; }

    /// <inheritdoc />
    public Type? ResponseValidatorType { get; }

    /// <inheritdoc />
    public object? Config => TypedConfig;
    public IResponseModelConfig<TResponse>? TypedConfig { get; }

    /// <inheritdoc />
    public object? ResponseValidator
    {
        get => TypedResponseValidator;
        set => TypedResponseValidator = (IResponseValidator<TResponse>?)value;
    }

    public IResponseModelConfig<TConfigResponse>? GetConfig<TConfigResponse>()
        where TConfigResponse : IUserResponse
    {
        if (!typeof(TResponse).IsEquivalentTo(typeof(TConfigResponse))) {
            throw new ArgumentException("Attempt to get a config for the type that is not " +
                                        "the declared response type for the model");
        }

        return (IResponseModelConfig<TConfigResponse>?)TypedConfig;
    }

    public IResponseValidator<TValidatorResponse>? GetValidator<TValidatorResponse>()
        where TValidatorResponse : IUserResponse
    {
        if (!typeof(TResponse).IsEquivalentTo(typeof(TValidatorResponse))) {
            throw new ArgumentException("Attempt to get a config for the type that is not " +
                                        "the declared response type for the model");
        }

        return (IResponseValidator<TValidatorResponse>?)TypedResponseValidator;
    }

    public IResponseValidator<TResponse>? TypedResponseValidator { get; private set; }
    
    public BasicResponseModel(string key, Type? responseParserType, 
        IResponseValidator<TResponse>? responseValidator)
    {
        // TODO: Validate parser type

        Key                    = key;
        Responded              = false;
        ResponseType           = typeof(TResponse);
        ResponseParserType     = responseParserType;
        TypedResponseValidator = responseValidator;
    }
    
    public BasicResponseModel(string key, Type? responseParserType, 
        Type? validatorType, IResponseModelConfig<TResponse>? config)
    {
        // TODO: Validate parser type
        // TODO: Validate validator type

        Key                   = key;
        Responded             = false;
        TypedConfig           = config;
        ResponseType          = typeof(TResponse);
        ResponseParserType    = responseParserType;
        ResponseValidatorType = validatorType;
    }
}
