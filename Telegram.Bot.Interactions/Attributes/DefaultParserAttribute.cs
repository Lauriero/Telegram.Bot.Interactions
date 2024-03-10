using Telegram.Bot.Interactions.Registry;

namespace Telegram.Bot.Interactions.Attributes;

/// <summary>
/// Marks the parser as the default one for the type.
/// Marking the parser with this attribute is equivalent to
/// adding the parser type with the <see cref="DefaultParserTypes.RegisterDefaultParserType{TResponse,TDefaultParser}"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DefaultParserAttribute : Attribute
{   
    public readonly Type ResponseType; 
    public DefaultParserAttribute(Type responseType)
    {
        // TODO: Add IUserResponse derived class validation 
        ResponseType = responseType;
    }
}