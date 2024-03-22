using System.Diagnostics.CodeAnalysis;

using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Validators;
using Telegram.Bot.Interactions.Validators.Configs;

namespace Telegram.Bot.Interactions.Model.Responses.Implementation;

/// <inheritdoc />
public class BasicResponseModel<TResponse> : IResponseModel<TResponse>
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
    public TResponse? Response { get; internal set; }

    public Type ResponseType { get; }

    /// <inheritdoc />
    public Type? ResponseParserType { get; set; }

    /// <inheritdoc />
    public Type? ResponseValidatorType { get; }
    
    /// <inheritdoc />
    public IResponseModelConfig<TResponse>? Config { get; }

    /// <inheritdoc />
    public IResponseValidator<TResponse>? ResponseValidator { get; }
    
    public BasicResponseModel(string key, Type? responseParserType, 
        IResponseValidator<TResponse>? responseValidator)
    {
        // TODO: Validate parser type

        Key          = key;
        Responded    = false;
        ResponseType = typeof(TResponse);
        ResponseValidator  = responseValidator;
        ResponseParserType = responseParserType;
    }
    
    public BasicResponseModel(string key, Type? responseParserType, 
        Type? validatorType, IResponseModelConfig<TResponse>? config)
    {
        // TODO: Validate parser type
        // TODO: Validate validator type

        Key                   = key;
        Responded             = false;
        Config                = config;
        ResponseValidatorType = validatorType;
        ResponseType          = typeof(TResponse);
        ResponseParserType    = responseParserType;
    }
}
