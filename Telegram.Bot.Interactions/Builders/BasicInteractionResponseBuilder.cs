using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Model.Responses.Descriptors;
using Telegram.Bot.Interactions.Model.Responses.Implementation;

namespace Telegram.Bot.Interactions.Builders;

/// <summary>
/// Is used to build instances of the <see cref="BasicInteractionResponseModel{TResponse}"/>.
/// By default builds the basic text response.
/// </summary>
public class BasicInteractionResponseBuilder<TResponse>
    where TResponse : IUserResponse
{
    private readonly string _key;
    private IInteractionResponseConfig<TResponse>? _config;

    public BasicInteractionResponseBuilder(string key)
    {
        _key = key;
    }

    /// <summary>
    /// Initiates the building process with the key of the build response.
    /// </summary>
    public static BasicInteractionResponseBuilder<TResponse> WithKey(string key)
    {
        return new BasicInteractionResponseBuilder<TResponse>(key);
    }
    
    /// <summary>
    /// Updates the config for the response.
    /// </summary>
    public BasicInteractionResponseBuilder<TResponse> WithConfig(
        IInteractionResponseConfig<TResponse> config)
    {
        _config = config;
        return this;
    }
    
    /// <summary>
    /// Build the response.
    /// </summary>
    public BasicInteractionResponseModel<TResponse> Build()
    {
        ResponseTypeMetadata typeMetadata = SmartInteractionResponseType.FromResponseType<TResponse>();
        return new BasicInteractionResponseModel<TResponse>(_key, typeMetadata.BasicType, _config);
    }
}