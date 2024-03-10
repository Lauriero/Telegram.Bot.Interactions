using System.Collections.ObjectModel;

using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Parsers;

namespace Telegram.Bot.Interactions.Registry;

public static class DefaultParserTypes
{
    private static readonly IDictionary<Type, Type> _entries = new Dictionary<Type, Type>();
    public static readonly IReadOnlyDictionary<Type, Type> Entries = new ReadOnlyDictionary<Type, Type>(_entries);

    /// <summary>
    /// Registers a parser with the type <typeparamref name="TDefaultParser"/>
    /// as a default parser for the response with the type <typeparamref name="TResponse"/>.
    /// </summary>
    public static void RegisterDefaultParserType<TResponse, TDefaultParser>()
        where TResponse : IUserResponse
        where TDefaultParser : IResponseParser<TResponse>
    {
        RegisterDefaultParserType(typeof(TResponse), typeof(TDefaultParser));
    }
    
    /// <summary>
    /// Registers a parser with the type <paramref name="defaultParserType"/>
    /// as a default parser for the response with the type <paramref name="responseType"/>.
    /// </summary>
    /// <param name="responseType">
    /// Should implement <see cref="IUserResponse"/> type.
    /// </param>
    /// <param name="defaultParserType">
    /// Should implement <see cref="IResponseParser{TResponse}"/> type
    /// with type parameter set to <paramref name="responseType"/>.
    /// </param>
    public static void RegisterDefaultParserType(Type responseType, Type defaultParserType)
    {
        // TODO: Add derivation validation
        _entries[responseType] = defaultParserType;
    }
}