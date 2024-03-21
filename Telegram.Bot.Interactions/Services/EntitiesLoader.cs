using System.Diagnostics.Contracts;
using System.Reflection;

using Microsoft.Extensions.Logging;

using Telegram.Bot.Interactions.Attributes.Modules;
using Telegram.Bot.Interactions.Attributes.Parsers;
using Telegram.Bot.Interactions.Exceptions.Modules;
using Telegram.Bot.Interactions.InteractionHandlers;
using Telegram.Bot.Interactions.Model;
using Telegram.Bot.Interactions.Model.Context;
using Telegram.Bot.Interactions.Model.Descriptors;
using Telegram.Bot.Interactions.Model.Descriptors.Loading;
using Telegram.Bot.Interactions.Model.Descriptors.Loading.Abstraction;
using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Parsers;
using Telegram.Bot.Interactions.Services.Abstraction;
using Telegram.Bot.Interactions.Utilities.DependencyInjection;
using Telegram.Bot.Interactions.Validators;

namespace Telegram.Bot.Interactions.Services;

public class EntitiesLoader : IEntitiesLoader
{
    private static readonly TypeInfo _moduleInterfaceType = 
        typeof(IInteractionModule).GetTypeInfo();
    
    internal IInteractionService InteractionService { private get; set; } = null!;
    
    private readonly IConfigurationService _config;
    private readonly ILoadedEntitiesRegistry _entitiesRegistry;
    private readonly ILogger<IEntitiesLoader> _logger;
    
    public EntitiesLoader(ILoadedEntitiesRegistry entitiesRegistry, 
        ILogger<IEntitiesLoader> logger, IConfigurationService config)
    {
        _logger           = logger;
        _config           = config;
        _entitiesRegistry = entitiesRegistry;
    }
    
    public MultipleLoadingResult<ModuleLoadingResult>
        LoadInteractionModules(Assembly interactionsAssembly,
            IServiceProvider? serviceProvider = null)
    {
        serviceProvider ??= new EmptyServiceProvider();
        try {
            IList<ModuleLoadingResult> loadingResults =
                BuildModuleInfos(interactionsAssembly, serviceProvider);

            foreach (ModuleLoadingResult moduleLoadingResult in loadingResults) {
                if (!moduleLoadingResult.Loaded) {
                    continue;
                }

                InteractionModuleInfo moduleInfo = moduleLoadingResult.Info;

                _entitiesRegistry.RegisterInteractionModule(moduleInfo);
            }

            return MultipleLoadingResult<ModuleLoadingResult>.FromSuccess(loadingResults);
        } catch (Exception e) {
            if (_config.StrictLoadingModeEnabled) {
                throw;
            }
            
            return MultipleLoadingResult<ModuleLoadingResult>.FromFailure(e);
        }
    }

    public GenericMultipleLoadingResult<ResponseParserInfo>
        LoadResponseParsers(Assembly parsersAssembly, 
            IServiceProvider? serviceProvider = null)
    {
        serviceProvider ??= new EmptyServiceProvider();
        List<GenericLoadingResult<ResponseParserInfo>> results = new();
        foreach (TypeInfo typeInfo in parsersAssembly.DefinedTypes) {
            if (!typeof(IResponseParser<IUserResponse>).IsAssignableFrom(typeInfo) 
                || typeof(IResponseParser<>).IsEquivalentTo(typeInfo)) {
                continue;
            }
            
            results.Add(LoadResponseParser(typeInfo, serviceProvider));
        }

        return new GenericMultipleLoadingResult<ResponseParserInfo>(results);
    }
    
    public GenericLoadingResult<ResponseParserInfo> 
        LoadResponseParser<TResponse, TParser>(IServiceProvider? serviceProvider = null)
            where TResponse : class, IUserResponse, new()
            where TParser : IResponseParser<TResponse>
    {
        return LoadResponseParser(typeof(TParser), serviceProvider);
    }
    
    private GenericLoadingResult<ResponseParserInfo>
        LoadResponseParser(Type parserType, IServiceProvider? serviceProvider = null)
    {
        serviceProvider ??= new EmptyServiceProvider();
        try {
            if (!IsValidParserDefinition(parserType)) {
                ParserLoadingException exception = new ParserLoadingException(parserType,
                    $"Parser definition should be a non-abstract public class, " +
                    $"but found class does not fit in these constrains");

                HandleSoftLoadingException(exception);
                return GenericLoadingResult<ResponseParserInfo>.FromFailure(parserType.Name,
                    exception);
            }

            bool isDefault = parserType.GetCustomAttribute<DefaultParserAttribute>() is not null;
            Type responseType = parserType
                .GetInterfaces()
                .First(i => 
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition()
                     .IsEquivalentTo(typeof(IResponseParser<>)))
                .GenericTypeArguments[0];
            
            if (responseType is not { IsClass: true, IsAbstract: false }) {
                ParserLoadingException exception = new ParserLoadingException(parserType,
                    $"Type of the response in the parser definition should be an " +
                    $"instantiable class");

                HandleSoftLoadingException(exception);
                return GenericLoadingResult<ResponseParserInfo>.FromFailure(parserType.Name,
                    exception);
            }

            IResponseParser<IUserResponse>? instance = (IResponseParser<IUserResponse>?)
                serviceProvider.GetService(parserType);

            if (instance is null) {
                if (!parserType.GetConstructors().Any(c => c.IsPublic && c.GetParameters().Length == 0)) {
                    ParserLoadingException exception = new ParserLoadingException(parserType,
                        "The parser should be either added to service provider's collection " +
                        "or should have a parameterless constructor in order to instantiate it");

                    HandleSoftLoadingException(exception);
                    return GenericLoadingResult<ResponseParserInfo>.FromFailure(parserType.Name,
                        exception);
                }

                try {
                    instance = (IResponseParser<IUserResponse>)Activator.CreateInstance(parserType)!;
                } catch (Exception e) {
                    ParserLoadingException exception = new ParserLoadingException(parserType, e.Message);
                    HandleSoftLoadingException(exception);
                    return GenericLoadingResult<ResponseParserInfo>.FromFailure(parserType.Name,
                        exception);
                }
            }

            ResponseParserInfo parserInfo = new ResponseParserInfo(parserType, responseType, 
                isDefault, instance);
            _entitiesRegistry.RegisterResponseParser(parserInfo);
            return GenericLoadingResult<ResponseParserInfo>.FromSuccess(parserType.Name, parserInfo);
        } catch (Exception e) {
            if (_config.StrictLoadingModeEnabled) {
                throw;
            }
            
            return GenericLoadingResult<ResponseParserInfo>.FromFailure(parserType.Name, e);
        }
    }

    public GenericMultipleLoadingResult<ResponseValidatorInfo> LoadResponseValidators(
        Assembly validatorsAssembly, IServiceProvider? serviceProvider = null)
    {
        serviceProvider ??= new EmptyServiceProvider();
        List<GenericLoadingResult<ResponseValidatorInfo>> results = new();
        foreach (TypeInfo typeInfo in validatorsAssembly.DefinedTypes) {
            if (!typeof(IResponseValidator<IUserResponse>).IsAssignableFrom(typeInfo) 
                || typeof(IResponseValidator<>).IsEquivalentTo(typeInfo)) {
                continue;
            }
            
            results.Add(LoadResponseValidator(typeInfo, serviceProvider));
        }

        return new GenericMultipleLoadingResult<ResponseValidatorInfo>(results);
    }

    public GenericLoadingResult<ResponseValidatorInfo> LoadResponseValidator<TResponse, TValidator>(
        IServiceProvider? _serviceProvider = null)
        where TResponse  : IUserResponse
        where TValidator : IResponseValidator<TResponse>
    {
        return LoadResponseValidator(typeof(TValidator));
    }
    
    public GenericLoadingResult<ResponseValidatorInfo> LoadResponseValidator(Type validatorType,
        IServiceProvider? _serviceProvider = null)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Scans <see cref="Assembly"/> to search for the interaction modules, loads them,
    /// saving metadata into proper descriptors, and returns them thereafter.
    /// </summary>
    /// <param name="assembly"><see cref="Assembly"/> to search for the modules.</param>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/> to use to create instances of the found modules.</param>
    private IList<ModuleLoadingResult> BuildModuleInfos(Assembly assembly, 
        IServiceProvider serviceProvider)
    {
        List<ModuleLoadingResult> results = new();
        foreach (TypeInfo typeInfo in SearchModules(assembly)) {
            if (!IsValidModuleDefinition(typeInfo)) {
                ModuleLoadingException exception = new ModuleLoadingException(typeInfo,
                    $"Module definition should be a non-nested non-abstract public class, " +
                    $"that doesn't have generic parameters, but found class does not fit "  +
                    $"in these constrains");

                HandleSoftLoadingException(exception);
                results.Add(ModuleLoadingResult.FromFailure(exception));
                continue;
            }
            
            IInteractionModule? instance = (IInteractionModule?)serviceProvider.GetService(typeInfo);
            if (instance is null) {
                if (!typeInfo.DeclaredConstructors.Any(
                        c => c.IsPublic && c.GetParameters().Length == 0)) {
                    ModuleLoadingException exception = new ModuleLoadingException(typeInfo,
                        "Module should be either added to service provider's collection " +
                        "or should have a parameterless constructor in order to instantiate it");
                    
                    HandleSoftLoadingException(exception);
                    results.Add(ModuleLoadingResult.FromFailure(exception));
                    continue;
                }

                try {
                    instance = (IInteractionModule)Activator.CreateInstance(typeInfo)!;
                } catch (Exception e) {
                    ModuleLoadingException exception = new ModuleLoadingException(typeInfo, e.Message);
                    HandleSoftLoadingException(exception);
                    results.Add(ModuleLoadingResult.FromFailure(exception));
                    continue;
                }
            }

            foreach (IInteraction interaction in instance.DeclareInteractions()) {
                _entitiesRegistry.RegisterInteraction(new InteractionInfo(interaction, null));
            }
            
            InteractionModuleInfo moduleInfo = new InteractionModuleInfo(
                typeInfo, InteractionService, serviceProvider, instance);
            
            List<InteractionHandlerInfo> handlerInfos = new List<InteractionHandlerInfo>();
            IList<GenericLoadingResult<InteractionHandlerInfo>> handlerInfosBuildingResult =
                BuildHandlerInfos(moduleInfo);
            foreach (GenericLoadingResult<InteractionHandlerInfo> result in handlerInfosBuildingResult) {
                if (!result.Loaded) {
                    continue;
                }

                handlerInfos.Add(result.Entity);
            }
            
            if (handlerInfos.Count == 0) {
                _logger.LogWarning("Module {moduleName} does not " + 
                                   "have any interaction handlers", typeInfo.FullName);
                continue;
            }
            
            moduleInfo.HandlerInfos.AddRange(handlerInfos);
            results.Add(ModuleLoadingResult.FromSuccess(moduleInfo, handlerInfosBuildingResult));
        }
        
        return results;
    }

    private IList<GenericLoadingResult<InteractionHandlerInfo>> BuildHandlerInfos(
        InteractionModuleInfo moduleInfo)
    {
        List<GenericLoadingResult<InteractionHandlerInfo>> results = new();
        foreach (MethodInfo methodInfo in moduleInfo.Type.GetMethods()) {
            InteractionHandlerAttribute? handlerAttribute =
                methodInfo.GetCustomAttribute<InteractionHandlerAttribute>();

            if (handlerAttribute is null) {
                continue;
            }

            if (!_entitiesRegistry.Interactions.ContainsKey(handlerAttribute.InteractionId)) {
                HandlerLoadingException exception = new HandlerLoadingException(moduleInfo.Type,
                    methodInfo, 
                    $"Interaction {handlerAttribute.InteractionId} was not registered " +
                    $"before the handler loading");

                HandleSoftLoadingException(exception);
                results.Add(GenericLoadingResult<InteractionHandlerInfo>.FromFailure(
                    methodInfo.Name, exception));
                continue;
            }

            InteractionInfo currentHandlerInteractionInfo =
                _entitiesRegistry.Interactions[handlerAttribute.InteractionId];
            
            if (currentHandlerInteractionInfo.HandlerInfo is not null) {
                HandlerLoadingException exception = new HandlerLoadingException(moduleInfo.Type,
                    methodInfo, $"The handler for the interaction {handlerAttribute.InteractionId} " +
                                $"was already found");

                HandleSoftLoadingException(exception);
                results.Add(GenericLoadingResult<InteractionHandlerInfo>.FromFailure(
                    methodInfo.Name, exception));
                continue;
            }

            if (!IsValidHandlerDefinition(methodInfo)) {
                HandlerLoadingException exception = new HandlerLoadingException(moduleInfo.Type,
                    methodInfo, 
                    "Handler definition should be a non-static, non-abstract "       +
                    "public method, that doesn't have generic parameters, but found method " +
                    "does not fit in these constraints");

                HandleSoftLoadingException(exception);
                results.Add(GenericLoadingResult<InteractionHandlerInfo>.FromFailure(
                    methodInfo.Name, exception));
                continue;
            }

            bool  isAsync;
            bool  isCancellable               = false;
            bool  acceptsSpecificContext      = false;
            Type? specificContextResponseType = null;
            
            if (methodInfo.ReturnType.IsEquivalentTo(typeof(void))) {
                isAsync = false;
            } else if (methodInfo.ReturnType.IsEquivalentTo(typeof(Task))) {
                isAsync = true;
            } else {
                HandlerLoadingException exception = new HandlerLoadingException(moduleInfo.Type,
                    methodInfo, 
                    "Handler method should only return void or Task, if used asynchronously");

                HandleSoftLoadingException(exception);
                results.Add(GenericLoadingResult<InteractionHandlerInfo>.FromFailure(
                    methodInfo.Name, exception));
                continue;
            }

            ParameterInfo[] handlerParams = methodInfo.GetParameters();
            if (handlerParams.Length is 0 or > 2) {
                HandlerLoadingException exception = new HandlerLoadingException(moduleInfo.Type,
                    methodInfo, 
                    "Handler method should have 1 or 2 parameters.");

                HandleSoftLoadingException(exception);
                results.Add(GenericLoadingResult<InteractionHandlerInfo>.FromFailure(
                    methodInfo.Name, exception));
                continue;
            }

            if (handlerParams.Length == 2) {
                if (handlerParams[1].ParameterType.IsEquivalentTo(typeof(CancellationToken))) {
                    isCancellable = true;
                } else {
                    HandlerLoadingException exception = new HandlerLoadingException(moduleInfo.Type,
                        methodInfo, 
                        "The second parameter of the handler method, if present, should " +
                        "be of the CancellationToken type");

                    HandleSoftLoadingException(exception);
                    results.Add(GenericLoadingResult<InteractionHandlerInfo>.FromFailure(
                        methodInfo.Name, exception));
                    continue;
                }
            }
            
            Type firstParamType = handlerParams[0].ParameterType;
            if (!firstParamType.IsGenericType 
                || firstParamType.GenericTypeArguments.Length == 0 
                || !firstParamType
                    .GetGenericTypeDefinition()
                    .IsEquivalentTo(typeof(IInteractionContext<>))) {
                HandlerLoadingException exception = new HandlerLoadingException(moduleInfo.Type,
                    methodInfo, 
                    "The first handler parameter should be of the IInteractionContext<> type");

                HandleSoftLoadingException(exception);
                results.Add(GenericLoadingResult<InteractionHandlerInfo>.FromFailure(methodInfo.Name, exception));
                continue;
            }

            if (firstParamType.GenericTypeArguments[0].IsEquivalentTo(typeof(IUserResponse))) {
                acceptsSpecificContext = false;
            } else {
                acceptsSpecificContext      = true;
                specificContextResponseType = firstParamType.GenericTypeArguments[0];
            }

            if (acceptsSpecificContext && (
                    currentHandlerInteractionInfo.Interaction.AvailableResponses.Count is 0 or > 1
                    || !currentHandlerInteractionInfo.Interaction.AvailableResponses[0]
                                                    .GetType()
                                                    .GenericTypeArguments[0]
                                                    .IsEquivalentTo(specificContextResponseType))) {
                HandlerLoadingException exception = new HandlerLoadingException(moduleInfo.Type,
                    methodInfo, 
                    "If the handler specifies response type, when declaring the context as "         +
                    "its first argument, the interaction it is assigned to should have no more then " +
                    "one response with the type that matches declared specific type");

                HandleSoftLoadingException(exception);
                results.Add(GenericLoadingResult<InteractionHandlerInfo>.FromFailure(methodInfo.Name, exception));
                continue;
            }
            
            InteractionHandlerInfo info = new InteractionHandlerInfo(
                handlerAttribute.InteractionId, handlerAttribute.RunMode, methodInfo, moduleInfo,
                acceptsSpecificContext, isAsync, isCancellable, specificContextResponseType);

            currentHandlerInteractionInfo.HandlerInfo = info;
            results.Add(GenericLoadingResult<InteractionHandlerInfo>.FromSuccess(info.Name, info));
        }

        return results;
    }
    
    private void HandleSoftLoadingException(Exception exception)
    {
        _logger.LogWarning(exception.Message);
        if (_config.StrictLoadingModeEnabled) {
            throw exception;
        }
    }
    
    [Pure]
    private static IEnumerable<TypeInfo> SearchModules(Assembly assembly)
    {
        List<TypeInfo> moduleTypes = new List<TypeInfo>();
        foreach (TypeInfo typeInfo in assembly.DefinedTypes) {
            if (!_moduleInterfaceType.IsAssignableFrom(typeInfo)) {
                continue;
            }

            moduleTypes.Add(typeInfo);
        }
        
        return moduleTypes;
    }

    [Pure]
    private static bool IsValidModuleDefinition(Type moduleType)
    {
        return moduleType is {
            IsPublic: true, IsClass: true, IsAbstract: false, IsNested: false,
            ContainsGenericParameters: false,
        };
    }

    [Pure]
    private static bool IsValidHandlerDefinition(MethodInfo methodInfo)
    {
        return methodInfo is {
            IsPublic: true, IsStatic: false, IsAbstract: false,
            ContainsGenericParameters: false
        };
    }

    [Pure]
    private static bool IsValidParserDefinition(Type parserType)
    {
        return parserType is {
            IsPublic: true, IsClass: true, IsAbstract: false,
        };
    }
    
    [Pure]
    private static bool IsValidValidatorDefinition(Type validatorType)
    {
        return validatorType is {
            IsPublic: true, IsClass: true, IsAbstract: false,
        };
    }
    
    
    
}