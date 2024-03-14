using System.Diagnostics.Contracts;
using System.Reflection;

using Microsoft.Extensions.Logging;

using Telegram.Bot.Interactions.Attributes.Modules;
using Telegram.Bot.Interactions.Exceptions.Modules;
using Telegram.Bot.Interactions.InteractionHandlers;
using Telegram.Bot.Interactions.Model;
using Telegram.Bot.Interactions.Model.Descriptors;
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
    
    private readonly SemaphoreSlim _lock;
    private readonly IConfigurationService _config;
    private readonly ILoadedEntitiesRegistry _entitiesRegistry;
    private readonly ILogger<IEntitiesLoader> _logger;
    
    public EntitiesLoader(ILoadedEntitiesRegistry entitiesRegistry, 
        ILogger<IEntitiesLoader> logger, IConfigurationService config)
    {
        _lock             = new SemaphoreSlim(1, 1);
        _entitiesRegistry = entitiesRegistry;
        _logger           = logger;
        _config      = config;
    }
    
    public async Task<GenericMultipleLoadingResult<InteractionModuleInfo>>
        LoadInteractionModulesAsync(Assembly interactionsAssembly,
            IServiceProvider? serviceProvider = null)
    {
        serviceProvider ??= new EmptyServiceProvider();
        await _lock.WaitAsync().ConfigureAwait(false);

        try {
            IList<GenericLoadingResult<InteractionModuleInfo>> loadingResults = 
                BuildModuleInfos(interactionsAssembly, serviceProvider);

            
            List<InteractionHandlerInfo> interactionHandlers = new();
            foreach (GenericLoadingResult<InteractionModuleInfo> moduleLoadingResult in loadingResults) {
                if (!moduleLoadingResult.Loaded) {
                    continue;
                }
                
                InteractionModuleInfo moduleInfo = moduleLoadingResult.Entity;
                
                _entitiesRegistry.RegisterInteractionModule(moduleInfo);
                foreach (IInteraction interaction in moduleInfo.Instance.DeclareInteractions()) {
                    _entitiesRegistry.RegisterInteraction(new InteractionInfo(interaction,
                        null));
                }
                
                interactionHandlers.AddRange(moduleInfo.HandlerInfos);
            }

            foreach (InteractionInfo info in _entitiesRegistry.Interactions.Values) {
                bool handlerFound = false;
                for (int i = 0; i < interactionHandlers.Count; i++) {
                    InteractionHandlerInfo handlerInfo = interactionHandlers[i];
                    if (info.Interaction.Id == handlerInfo.InteractionId) {
                        handlerFound = true;
                        info.Handler = handlerInfo;
                        interactionHandlers.RemoveAt(i);
                        break;
                    }
                }

                if (!handlerFound) {
                    _logger.LogWarning("Handler for the interaction {id} was not found, " +
                                       "the interaction will not be handled", info.Interaction.Id);
                }
            }

            foreach (InteractionHandlerInfo info in interactionHandlers) {
                _logger.LogWarning("Interaction handler was left without ");
            }
            
            return new GenericMultipleLoadingResult<InteractionModuleInfo>(loadingResults);
        } finally {
            _lock.Release();
        }
    }

    public Task<GenericLoadingResult<ResponseValidatorInfo>> 
        LoadResponseValidatorAsync<TResponse, TValidator>(IServiceProvider? serviceProvider = null)
            where TResponse : class, IUserResponse, new()
            where TValidator : IResponseValidator<TResponse>
    {
        throw new NotImplementedException();
    }

    public Task<GenericMultipleLoadingResult<ResponseValidatorInfo>> 
        LoadResponseParserAsync<TResponse, TParser>(IServiceProvider? serviceProvider = null)
            where TResponse : class, IUserResponse, new()
            where TParser : IResponseParser<TResponse>
    {
        throw new NotImplementedException();
    }
    
    
    /// <summary>
    /// Scans <see cref="Assembly"/> to search for the interaction modules, loads them,
    /// saving metadata into proper descriptors, and returns them thereafter.
    /// </summary>
    /// <param name="assembly"><see cref="Assembly"/> to search for the modules.</param>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/> to use to create instances of the found modules.</param>
    private IList<GenericLoadingResult<InteractionModuleInfo>> BuildModuleInfos(Assembly assembly, 
        IServiceProvider serviceProvider)
    {
        List<int> loadedHandlerInteractionIds = new();
        List<GenericLoadingResult<InteractionModuleInfo>> results = new();
        foreach (TypeInfo typeInfo in SearchModules(assembly)) {
            if (!IsValidModuleDefinition(typeInfo)) {
                ModuleLoadingException exception = new ModuleLoadingException(typeInfo,
                    $"Module definition should be a non-nested non-abstract public class, " +
                    $"that doesn't have generic parameters, but found class does not fit "  +
                    $"in these constrains");

                HandleSoftLoadingException(exception);
                results.Add(GenericLoadingResult<InteractionModuleInfo>.FromFailure(exception));
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
                    results.Add(GenericLoadingResult<InteractionModuleInfo>.FromFailure(exception));
                    continue;
                }

                try {
                    instance = (IInteractionModule)Activator.CreateInstance(typeInfo)!;
                } catch (Exception e) {
                    ModuleLoadingException exception = new ModuleLoadingException(typeInfo, e.Message);
                    HandleSoftLoadingException(exception);
                    results.Add(GenericLoadingResult<InteractionModuleInfo>.FromFailure(exception));
                    continue;
                }
            }
            
            InteractionModuleInfo moduleInfo = new InteractionModuleInfo(
                typeInfo, InteractionService, serviceProvider, instance);
            
            List<InteractionHandlerInfo> handlerInfos = new List<InteractionHandlerInfo>();
            foreach (GenericLoadingResult<InteractionHandlerInfo> result in BuildHandlerInfos(moduleInfo)) {
                if (!result.Loaded) {
                    continue;
                }

                InteractionHandlerInfo handlerInfo = result.Entity;
                if (loadedHandlerInteractionIds.Contains(result.Entity.InteractionId)) {
                    HandleSoftLoadingException(new HandlerLoadingException(
                        handlerInfo.Module.Type, handlerInfo.MethodInfo, 
                        ""));
                }
                
                handlerInfos.Add(result.Entity);
            }
            
            if (handlerInfos.Count == 0) {
                _logger.LogWarning("Module {moduleName} does not " + 
                                   "have any interaction handlers", typeInfo.FullName);
                continue;
            }
            
            moduleInfo.HandlerInfos.AddRange(handlerInfos);
            results.Add(GenericLoadingResult<InteractionModuleInfo>.FromSuccess(moduleInfo));
        }
        
        return results;
    }

    private IEnumerable<GenericLoadingResult<InteractionHandlerInfo>> BuildHandlerInfos(
        InteractionModuleInfo moduleInfo)
    {
        List<GenericLoadingResult<InteractionHandlerInfo>> results = new();
        foreach (MethodInfo methodInfo in moduleInfo.Type.GetMethods()) {
            InteractionHandlerAttribute? handlerAttribute =
                methodInfo.GetCustomAttribute<InteractionHandlerAttribute>();

            if (handlerAttribute is null) {
                continue;
            }

            if (!IsValidHandlerDefinition(methodInfo)) {
                HandlerLoadingException exception = new HandlerLoadingException(moduleInfo.Type,
                    methodInfo, 
                    "Handler definition should be a non-static, non-abstract "       +
                    "public method, that doesn't have generic parameters, but found method " +
                    "does not fit in these constraints");

                HandleSoftLoadingException(exception);
                results.Add(GenericLoadingResult<InteractionHandlerInfo>.FromFailure(exception));
                continue;
            }
            
            InteractionHandlerInfo info = new InteractionHandlerInfo(
                handlerAttribute.InteractionId, handlerAttribute.RunMode, methodInfo, moduleInfo);

            results.Add(GenericLoadingResult<InteractionHandlerInfo>.FromSuccess(info));
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
    private static bool IsValidModuleDefinition(TypeInfo typeInfo)
    {
        return typeInfo is {
            IsPublic: true, IsAbstract: false, IsNested: false,
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
    
    
}