using Telegram.Bot.Interactions.Registry;

namespace Telegram.Bot.Interactions.Attributes;

/// <summary>
/// Marks the validator as the default one for the type.
/// Marking the validator with this attribute is equivalent to
/// adding the parser type with the <see cref="DefaultValidatorTypes.RegisterDefaultValidatorType{TResponse,TDefaultParser}"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DefaultValidatorAttribute : Attribute
{
    public readonly Type ResponseType; 
    public DefaultValidatorAttribute(Type responseType)
    {
        // TODO: Add IUserResponse derived class validation 
        ResponseType = responseType;
    }
}