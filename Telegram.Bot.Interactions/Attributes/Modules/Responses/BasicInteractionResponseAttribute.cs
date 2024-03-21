using Telegram.Bot.Interactions.Builders.InteractionResponses;
using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Parsers;

namespace Telegram.Bot.Interactions.Attributes.Modules.Responses;

/// <summary>
/// Adds the basic response as an available response for an interaction.
/// Is equivalent to using <see cref="BasicResponseModelBuilder{TResponse}"/>,
/// providing the response type and the optional parser.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class BasicInteractionResponseAttribute : BaseResponseAttribute
{
    /// <summary>
    /// Registers a response.
    /// </summary>
    /// <param name="key"><see cref="IResponseModel{TResponse}.Key"/> value</param>
    /// <param name="responseType">
    /// Is equivalent to TResponse type in <see cref="IResponseModel{TResponse}"/>
    /// </param>
    /// <param name="parserType">
    /// Should be a type that implements <see cref="IResponseParser{TResponse}"/>
    /// with TResponse set to <paramref name="responseType"/>.
    /// </param>
    public BasicInteractionResponseAttribute(string key, Type responseType, Type? parserType = null) 
        : base(key, responseType, parserType, null)
    {
    }
}