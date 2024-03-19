using System.Diagnostics.CodeAnalysis;

using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Validators;

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
    
    /// <inheritdoc />
    public Type? ResponseParserType { get; }

    /// <inheritdoc />
    public IResponseValidator<TResponse>? ResponseValidator { get; }
    
    internal BasicResponseModel(string key, Type? responseParserType, 
        IResponseValidator<TResponse>? responseValidator)
    {
        // TODO: Validate parser type

        Key                = key;
        Responded          = false;
        ResponseValidator  = responseValidator;
        ResponseParserType = responseParserType;
    }
}
