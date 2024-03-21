using System.Reflection;

using Telegram.Bot.Interactions.Exceptions;
using Telegram.Bot.Interactions.Exceptions.Modules;
using Telegram.Bot.Interactions.InteractionHandlers;
using Telegram.Bot.Interactions.Model;
using Telegram.Bot.Interactions.Model.Descriptors;
using Telegram.Bot.Interactions.Model.Descriptors.Loading;
using Telegram.Bot.Interactions.Model.Descriptors.Loading.Abstraction;
using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Parsers;
using Telegram.Bot.Interactions.Validators;

namespace Telegram.Bot.Interactions.Services.Abstraction;

/// <summary>
/// Service that loads interaction modules, parsers,
/// validators etc. and stores the metadata about them.
/// </summary>
public interface IEntitiesLoader
{
    /// <summary>
    /// Loads interaction modules - classes that should derive from
    /// <see cref="IInteractionModule"/> and will be used to handle
    /// registered instances of <see cref="IInteraction"/> between
    /// the bot and the user.
    /// </summary>
    /// <param name="interactionsAssembly">
    /// The assembly instances, interaction modules are located at, that
    /// will be scanned and the located modules with its handlers will be loaded.  
    /// </param>
    /// <param name="serviceProvider">
    /// Service provider that will be used to create new instances of registered
    /// interaction modules, and has to have the modules registered.
    /// If not provided, empty provider will be used.
    /// </param>
    /// <remarks>
    /// This method will try resolve interaction modules so be sure to register
    /// any dependencies in provided <see cref="IServiceProvider"/> beforehand,
    /// if you consider to use dependency injection.
    /// If not, set the serviceProvider to null.
    /// </remarks>
    /// <returns>
    /// Result of the loading that contains both loaded, and not loaded instances.
    /// </returns>
    /// <exception cref="ModuleLoadingException">
    /// Is occurred on loading errors related to the module being not-properly
    /// defined if the <see cref="IConfigurationService.StrictLoadingModeEnabled"/> is set to true.
    /// </exception>
    /// <exception cref="HandlerLoadingException">
    /// Is occurred on loading errors related to the module handlers being not-properly
    /// defined if the <see cref="IConfigurationService.StrictLoadingModeEnabled"/> is set to true.
    /// </exception>
    /// <exception cref="ParserNotRegisteredException{TResponse}">
    /// Is occurred when the default parser for the type, that is declared as a response
    /// type of one of the interactions declared in <see cref="IInteractionModule.DeclareInteractions"/>,
    /// was not previously registered via the <see cref="LoadResponseParser{TResponse,TParser}"/>.
    /// </exception>
    public MultipleLoadingResult<ModuleLoadingResult> 
        LoadInteractionModules(Assembly interactionsAssembly, 
            IServiceProvider? serviceProvider = null);

    public GenericMultipleLoadingResult<ResponseParserInfo>
        LoadResponseParsers(Assembly parsersAssembly,
            IServiceProvider? serviceProvider = null);
    
    public GenericLoadingResult<ResponseParserInfo>
        LoadResponseParser<TResponse, TParser>(IServiceProvider? serviceProvider = null)
        where TResponse : class, IUserResponse, new()
        where TParser : IResponseParser<TResponse>;

    public GenericMultipleLoadingResult<ResponseValidatorInfo> LoadResponseValidators(
        Assembly validatorsAssembly,
        IServiceProvider? serviceProvider = null);

    public GenericLoadingResult<ResponseValidatorInfo> LoadResponseValidator<TResponse, TValidator>(
        IServiceProvider? serviceProvider = null)
        where TResponse : IUserResponse
        where TValidator : IResponseValidator<TResponse>;

    public GenericLoadingResult<ResponseValidatorInfo> LoadResponseValidator(Type validatorType,
        IServiceProvider? serviceProvider = null);
}