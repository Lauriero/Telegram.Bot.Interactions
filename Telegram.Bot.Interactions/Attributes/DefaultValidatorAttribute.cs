using Telegram.Bot.Interactions.Services.Abstraction;

namespace Telegram.Bot.Interactions.Attributes;

/// <summary>
/// Marks the validator as the default one for the type.
/// Marking the validator with this attribute is equivalent to
/// adding the validator type with the <see cref="IEntitiesLoader"/> 
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DefaultValidatorAttribute : Attribute
{
    public readonly Type ResponseType; 
    public DefaultValidatorAttribute(Type responseType)
    {
        // TODO: Add IUserResponse derived class validation 
        ResponseType = responseType;
    }
}