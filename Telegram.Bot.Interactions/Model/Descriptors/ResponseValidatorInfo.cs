using System.Collections.ObjectModel;

using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Validators;
using Telegram.Bot.Interactions.Validators.Configs;

namespace Telegram.Bot.Interactions.Model.Descriptors;

/// <summary>
/// Contains information about the response validator.
/// Gets generated when the validator is loaded into the interactions service. 
/// </summary>
public class ResponseValidatorInfo
{
    /// <summary>
    /// Type of the validator that implements the <see cref="IResponseValidator{TResponse}"/> type.
    /// </summary>
    public Type ValidatorType { get; }
    
    /// <summary>
    /// Type of the response that implements the <see cref="IUserResponse"/> class
    /// and gets validated using the validator.
    /// </summary>
    public Type ResponseType { get; }
    
    /// <summary>
    /// List of types that implement <see cref="IResponseModelConfig{TResponse}"/> type
    /// and are marked as available for the validator using ConfigurableWith attributes.
    /// </summary>
    public IReadOnlyList<Type> AvailableConfigTypes { get; }
    
    public ResponseValidatorInfo(Type validatorType, Type responseType, IList<Type> availableConfigTypes)
    {
        ResponseType         = responseType;
        ValidatorType        = validatorType;
        AvailableConfigTypes = new ReadOnlyCollection<Type>(availableConfigTypes);
    }
}