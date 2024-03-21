using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Validators;

namespace Telegram.Bot.Interactions.Attributes.Modules.Responses;

public abstract class BaseResponseAttribute : Attribute
{
    public string Key { get; }
    public Type ResponseType { get; }
    public Type? ParserType { get; }
    public Type? ValidatorType { get; }

    /// <summary>
    /// The validator factory that is used to instantiate validator
    /// based on provided arguments in a derived attributes.
    /// This factory is optional if the <see cref="ValidatorType"/> is the type
    /// that has a parameterless constructor or is resolved using the DI containers
    /// and thus, doesn't need to be explicitly instantiated.
    /// </summary>
    /// <returns></returns>
    public virtual IResponseValidator<IUserResponse>? CreateValidator()
    {
        return null;
    }
    
    protected BaseResponseAttribute(string key, Type responseType, Type? parserType,
        Type? validatorType)
    {
        Key           = key;
        ParserType    = parserType;
        ResponseType  = responseType;
        ValidatorType = validatorType;
    }
}