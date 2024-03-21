using Telegram.Bot.Interactions.Exceptions.Interactions;
using Telegram.Bot.Interactions.Model.Responses.Abstraction;

namespace Telegram.Bot.Interactions.Attributes.Parsers;

/// <summary>
/// Specifies that the validator can accept the provided type of config.
/// If the type of <see cref="IResponseModel{TResponse}.Config"/> hasn't been declared
/// as the valid config for the <see cref="IResponseModel{TResponse}.ResponseValidatorType"/>,
/// the <see cref="ConfigurationNotSupportedException{TResponse}"/> will be thrown.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ConfigurableWithAttribute : Attribute
{
    public Type? ConfigType { get; }
    
    public ConfigurableWithAttribute(Type? configType) { ConfigType = configType; }
}