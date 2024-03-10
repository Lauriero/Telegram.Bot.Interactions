using System.Collections.ObjectModel;

using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Validators;

namespace Telegram.Bot.Interactions.Registry;

public class DefaultValidatorTypes
{
    private static readonly IDictionary<Type, Type> _entries = new Dictionary<Type, Type>();
    public static readonly IReadOnlyDictionary<Type, Type> Entries = new ReadOnlyDictionary<Type, Type>(_entries);

    /// <summary>
    /// Registers a validator with the type <typeparamref name="TDefaultValidator"/>
    /// as a default validator for the response with the type <typeparamref name="TResponse"/>.
    /// </summary>
    public static void RegisterDefaultValidatorType<TResponse, TDefaultValidator>()
        where TResponse : IUserResponse
        where TDefaultValidator : IResponseValidator<TResponse>
    {
        RegisterDefaultValidatorType(typeof(TResponse), typeof(TDefaultValidator));
    }
    
    /// <summary>
    /// Registers a validator with the type <paramref name="defaultValidatorType"/>
    /// as a default validator for the response with the type <paramref name="responseType"/>.
    /// </summary>
    /// <param name="responseType">
    /// Should implement <see cref="IUserResponse"/> type.
    /// </param>
    /// <param name="defaultValidatorType">
    /// Should implement <see cref="IResponseValidator{TResponse}"/> type
    /// with type parameter set to <paramref name="responseType"/>.
    /// </param>
    public static void RegisterDefaultValidatorType(Type responseType, Type defaultValidatorType)
    {
        // TODO: Add derivation validation
        _entries[responseType] = defaultValidatorType;
    }
}