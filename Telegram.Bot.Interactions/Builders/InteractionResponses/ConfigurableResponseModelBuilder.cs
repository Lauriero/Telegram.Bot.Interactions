using Telegram.Bot.Interactions.Builders.Configs;
using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Model.Responses.Implementation;
using Telegram.Bot.Interactions.Validators;

namespace Telegram.Bot.Interactions.Builders.InteractionResponses;

public class ConfigurableResponseModelBuilder<TResponse> : BasicResponseModelBuilder<TResponse>
    where TResponse : class, IUserResponse, new()
{
    private Type? _validatorType;
    private readonly IResponseModelConfig<TResponse> _config;

    private ConfigurableResponseModelBuilder(string key, IResponseModelConfig<TResponse> config) 
        : base(key)
    {
        _config = config;
    }
    
    /// <summary>
    /// Initiates the building process with the key of the build response and the config.
    /// </summary>
    public static ConfigurableResponseModelBuilder<TResponse> WithKeyAndConfig(string key,
        IResponseModelConfig<TResponse> config)
    {
        return new ConfigurableResponseModelBuilder<TResponse>(key, config);
    }
    
    /// <summary>
    /// Initiates the building process with the key of the build response and the config builder.
    /// </summary>
    public static ConfigurableResponseModelBuilder<TResponse> WithKeyAndConfig(string key,
        IResponseModelConfigBuilder<TResponse> configBuilder)
    {
        return new ConfigurableResponseModelBuilder<TResponse>(key, configBuilder.Build());
    }
    
    /// <summary>
    /// Sets the built response validator to the specified value.
    /// </summary>
    /// <param name="validatorType">
    /// Should implement <see cref="IResponseValidator{TResponse}"/>
    /// with the <see cref="TResponse"/> type parameter.
    /// </param>
    public ConfigurableResponseModelBuilder<TResponse> WithValidator(Type validatorType)
    {
        _validatorType = validatorType;
        return this;
    } 

    /// <inheritdoc cref="WithValidator"/>
    public ConfigurableResponseModelBuilder<TResponse> WithValidator<TValidator>()
        where TValidator : IResponseValidator<TResponse>
    {
        return WithValidator(typeof(TValidator));
    }

    public new ConfigurableResponseModel<TResponse> Build()
    {
        return new ConfigurableResponseModel<TResponse>(_key, _parserType, _config, _validatorType);
    }
}