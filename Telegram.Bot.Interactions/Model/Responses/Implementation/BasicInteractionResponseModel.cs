using System.Diagnostics.CodeAnalysis;

using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Model.Responses.Descriptors;

namespace Telegram.Bot.Interactions.Model.Responses.Implementation;

/// <inheritdoc />
public class BasicInteractionResponseModel<TResponse> : IInteractionResponseModel<TResponse>
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
    public InteractionResponseType Type { get; }

    /// <inheritdoc />
    public TResponse? Response { get; }

    /// <inheritdoc />
    public IInteractionResponseConfig<TResponse>? Config { get; }
    
    public BasicInteractionResponseModel(string key, InteractionResponseType type, 
        IInteractionResponseConfig<TResponse>? config = null)
    {
        Key = key;
        Type = type;
        Config = config;
        Responded = false;
    }

    public static IInteractionResponseModel<IUserResponse> 
        ToIInteractionResponseModel(BasicInteractionResponseModel<TResponse> response)
    {
        return (IInteractionResponseModel<IUserResponse>) response;
    }
}