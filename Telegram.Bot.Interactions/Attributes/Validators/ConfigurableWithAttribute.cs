using Telegram.Bot.Interactions.Exceptions.Interactions;
using Telegram.Bot.Interactions.Model.Responses.Implementation;

namespace Telegram.Bot.Interactions.Attributes.Validators;

/// <summary>
/// Specifies that the validator can accept the provided type of config.
/// If the type of <see cref="BasicResponseModel{TResponse}.Config"/> hasn't been declared
/// as the valid config for the <see cref="BasicResponseModel{TResponse}.ResponseValidatorType"/>,
/// the <see cref="ConfigurationNotSupportedException{TResponse}"/> will be thrown.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ConfigurableWithAttribute : Attribute
{
    public Type ConfigType { get; }
    
    public ConfigurableWithAttribute(Type configType) { ConfigType = configType; }
}