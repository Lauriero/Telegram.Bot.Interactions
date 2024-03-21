using Telegram.Bot.Interactions.Builders.InteractionResponses;

namespace Telegram.Bot.Interactions.Attributes.Modules.Responses;

/// <summary>
/// Adds the basic response as an available response for an interaction.
/// Is equivalent to using <see cref="BasicResponseModelBuilder{TResponse}"/>,
/// providing the response type and the optional parser.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ConfigurableInteractionResponseAttribute : BaseResponseAttribute
{
    public ConfigurableInteractionResponseAttribute(string key, Type responseType, Type validatorType,
        Type? parserType = null) : base(key, responseType, parserType, validatorType)
    {
    }
}