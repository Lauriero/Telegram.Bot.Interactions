using System.Diagnostics.CodeAnalysis;

using Telegram.Bot.Interactions.Exceptions;
using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Model.Responses.Implementation;
using Telegram.Bot.Interactions.Parsers;

namespace Telegram.Bot.Interactions.Builders.InteractionResponses;

/// <summary>
/// Is used to build instances of the <see cref="BasicResponseModel{TResponse}"/>.
/// </summary>
public class BasicResponseModelBuilder<TResponse> : IResponseModelBuilder<TResponse>
    where TResponse : class, IUserResponse, new()
{
    protected readonly string _key;
    protected Type? _parserType;

    protected BasicResponseModelBuilder(string key)
    {
        _key = key;
    }

    /// <summary>
    /// Initiates the building process with the key of the build response.
    /// </summary>
    public static BasicResponseModelBuilder<TResponse> WithKey(string key)
    {
        return new BasicResponseModelBuilder<TResponse>(key);
    }

    /// <inheritdoc cref="WithParser(System.Type)"/>
    public BasicResponseModelBuilder<TResponse> WithParser<TParser>()
        where TParser : IResponseParser<TResponse>
    {
        return WithParser(typeof(TParser));
    } 
    
    /// <summary>
    /// Sets the built response model parser type to the specified value.
    /// </summary>
    public BasicResponseModelBuilder<TResponse> WithParser(Type parserType)
    {
        _parserType = parserType;
        return this;
    }

    /// <summary>
    /// Build the response.
    /// </summary>
    public IResponseModel<TResponse> Build()
    {
        return new BasicResponseModel<TResponse>(_key, _parserType);
    }
}