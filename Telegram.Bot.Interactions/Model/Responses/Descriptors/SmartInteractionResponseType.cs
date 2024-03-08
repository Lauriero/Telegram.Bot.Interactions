using Ardalis.SmartEnum;

using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Model.Responses.Implementation.Configs;
using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;

namespace Telegram.Bot.Interactions.Model.Responses.Descriptors;

/// <summary>
/// Enumeration of the response factories that are attached to the
/// <see cref="InteractionResponseType"/> that they create. 
/// </summary>
public class SmartInteractionResponseType : SmartEnum<SmartInteractionResponseType, ResponseTypeMetadata>
{
    public static readonly SmartInteractionResponseType Text = new SmartInteractionResponseType(nameof(Text), 
        new ResponseTypeMetadata(InteractionResponseType.Text, typeof(TextResponseConfig), typeof(TextResponse)));

    /// <summary>
    /// Finds the member with the specified basic type.
    /// </summary>
    /// <param name="type">
    /// Type declared in <see cref="ResponseTypeMetadata.BasicType"/>.
    /// </param>
    public static SmartInteractionResponseType FromBasicType(InteractionResponseType type)
    {
        return List.First(t => t.Value.BasicType == type);
    }

    /// <summary>
    /// Finds the member with the specified response type. 
    /// </summary>
    /// <typeparam name="TResponse">
    /// Type declared in <see cref="ResponseTypeMetadata.ResponseType"/>.
    /// </typeparam>
    public static SmartInteractionResponseType FromResponseType<TResponse>()
        where TResponse : IUserResponse
    {
        return List.First(t => t.Value.ResponseType == typeof(TResponse));
    }
    
    // TODO: Implement safe methods
    
    public SmartInteractionResponseType(string name, ResponseTypeMetadata value) : base(name, value) { }
}