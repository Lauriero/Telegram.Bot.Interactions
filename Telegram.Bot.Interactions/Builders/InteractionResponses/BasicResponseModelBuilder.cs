using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Model.Responses.Implementation;
using Telegram.Bot.Interactions.Parsers;
using Telegram.Bot.Interactions.Validators;
using Telegram.Bot.Interactions.Validators.Configs;

namespace Telegram.Bot.Interactions.Builders.InteractionResponses;

/// <summary>
/// Is used to build instances of the <see cref="BasicResponseModel{TResponse}"/>.
/// </summary>
public class BasicResponseModelBuilder<TResponse> : IResponseModelBuilder<TResponse>
    where TResponse : class, IUserResponse, new()
{
    private Type? _parserType;
    private Type? _validatorType;
    private readonly string _key;
    private IResponseModelConfig<TResponse>? _config;
    private IResponseValidator<TResponse>? _validator;
    
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
    /// Sets the <see cref="IResponseModel{TResponse}.ResponseValidator"/>
    /// to the specified value.
    /// </summary>
    public BasicResponseModelBuilder<TResponse> WithValidator(
        IResponseValidator<TResponse> validator)
    {
        _validator = validator;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="IResponseModel{TResponse}.ResponseValidatorType"/>.
    /// </summary>
    public BasicResponseModelBuilder<TResponse> WithValidator<TValidator>()
        where TValidator : IResponseValidator<TResponse>
    {
        _validatorType = typeof(TValidator);
        return this;
    }

    /// <summary>
    /// Sets the <see cref="IResponseModel{TResponse}.Config"/>.
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public BasicResponseModelBuilder<TResponse> WithConfig(IResponseModelConfig<TResponse> config)
    {
        _config = config;
        return this;
    }


    /// <summary>
    /// Build the response.
    /// </summary>
    public IResponseModel<TResponse> Build()
    {
        return _validatorType is not null 
            ? new BasicResponseModel<TResponse>(_key, _parserType, _validatorType, _config) 
            : new BasicResponseModel<TResponse>(_key, _parserType, _validator);
    }
}