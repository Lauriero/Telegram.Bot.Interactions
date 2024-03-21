using Telegram.Bot.Interactions.Exceptions.Interactions;
using Telegram.Bot.Interactions.Model.Responses.Abstraction;

namespace Telegram.Bot.Interactions.Attributes.Parsers;

/// <summary>
/// Specifies that the validator can accept all the config type, as long as the type of config
/// has a type parameter set to the response the validator validates.
/// provided type of config.
/// If the type of <see cref="IResponseModel{TResponse}.Config"/> hasn't been declared
/// as the valid config for the <see cref="IResponseModel{TResponse}.ResponseValidatorType"/>,
/// the <see cref="ConfigurationNotSupportedException{TResponse}"/> will be thrown.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ConfigurableWithAnyOfMyTypeAttribute : Attribute
{
}