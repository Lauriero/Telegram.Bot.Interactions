using Telegram.Bot.Interactions.Services.Abstraction;

namespace Telegram.Bot.Interactions.Attributes;

/// <summary>
/// Marks the parser as the default one for the type.
/// Marking the parser with this attribute is equivalent to loading it
/// using the <see cref="IEntitiesLoader"/>. // TODO: specify method in loader
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