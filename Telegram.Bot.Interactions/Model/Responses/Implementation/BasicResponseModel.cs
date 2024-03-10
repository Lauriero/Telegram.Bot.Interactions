using System.Diagnostics.CodeAnalysis;

using Telegram.Bot.Interactions.Model.Responses.Abstraction;

namespace Telegram.Bot.Interactions.Model.Responses.Implementation;

/// <summary>
/// Basic response model that uses response type,
/// provided in <typeparamref name="TResponse"/>
/// to determine valid response parameters.
/// </summary>
public class BasicResponseModel<TResponse> : IResponseModel<TResponse>
    where TResponse : class, IUserResponse, new()
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

    public BasicResponseModel(string key, Type responseParserType)
    {
        Key = key;
        Responded = false;
        ResponseParserType = responseParserType;
    }
}