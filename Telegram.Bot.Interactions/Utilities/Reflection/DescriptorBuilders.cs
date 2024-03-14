using System.Reflection;

using Microsoft.Extensions.Logging;

using Telegram.Bot.Interactions.Attributes.Modules;
using Telegram.Bot.Interactions.Exceptions.Modules;
using Telegram.Bot.Interactions.InteractionHandlers;
using Telegram.Bot.Interactions.Model.Descriptors;
using Telegram.Bot.Interactions.Model.Descriptors.Loading;
using Telegram.Bot.Interactions.Utilities.Extensions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Interactions.Utilities.Reflection;

internal static class DescriptorBuilders
{
    private static readonly TypeInfo _moduleInterfaceType = 
        typeof(IInteractionModule).GetTypeInfo();

    /// <summary>
    /// Scans <see cref="Assembly"/> to search for the interaction modules, loads them,
    /// saving metadata into proper descriptors, and returns them thereafter.
    /// </summary>
    /// <param name="assembly"><see cref="Assembly"/> to search for the modules.</param>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/> to use to create instances of the found modules.</param>
    /// <param name="interactionService">Requesting service reference</param>
    public static IList<ModuleLoadingResult> BuildModuleInfos(Assembly assembly, 
        IServiceProvider serviceProvider, IInteractionService interactionService)
    {
        List<ModuleLoadingResult> results = new List<ModuleLoadingResult>();
        foreach (TypeInfo typeInfo in SearchModules(assembly, interactionService)) {
            if (!IsValidModuleDefinition(typeInfo)) {
                ModuleLoadingException exception = new ModuleLoadingException(typeInfo,
                    $"Module definition should be a non-nested non-abstract public class, " +
                    $"that doesn't have generic parameters, but found class does not fit "  +
                    $"in these constrains");

                interactionService.HandleSoftLoadingException(exception);
                results.Add(new ModuleLoadingResult(false, 
                    loadingException: exception));
                continue;
            }
            
            IInteractionModule? instance = (IInteractionModule?)serviceProvider.GetService(typeInfo);
            if (instance is null) {
                if (!typeInfo.DeclaredConstructors.Any(
                        c => c.IsPublic && c.GetParameters().Length == 0)) {
                    ModuleLoadingException exception = new ModuleLoadingException(typeInfo,
                        "Module should be either added to service provider's collection " +
                        "or should have a parameterless constructor in order to instantiate it");
                    
                    interactionService.HandleSoftLoadingException(exception);
                    results.Add(new ModuleLoadingResult(false, loadingException: exception));
                    continue;
                }

                try {
                    instance = (IInteractionModule)Activator.CreateInstance(typeInfo)!;
                } catch (Exception e) {
                    ModuleLoadingException exception = new ModuleLoadingException(typeInfo, e.Message);
                    interactionService.HandleSoftLoadingException(exception);
                    results.Add(new ModuleLoadingResult(false, loadingException: exception));
                    continue;
                }
            }
            
            InteractionModuleInfo moduleInfo = new InteractionModuleInfo(
                typeInfo, interactionService, serviceProvider, instance);
            
            List<InteractionHandlerInfo> handlerInfos = new List<InteractionHandlerInfo>();
            foreach (HandlerLoadingResult result in BuildHandlerInfos(moduleInfo, interactionService)) {
                if (result.Loaded) {
                    handlerInfos.Add(result.Info);
                }
            }
            
            if (handlerInfos.Count == 0) {
                interactionService.Logger.LogWarning("Module {moduleName} does not " +
                                                     "have any interaction handlers", typeInfo.FullName);
                continue;
            }
            
            moduleInfo.HandlerInfos.AddRange(handlerInfos);
            results.Add(new ModuleLoadingResult(true, moduleInfo));
        }
        
        return results;
    }

    private static IEnumerable<HandlerLoadingResult> BuildHandlerInfos(InteractionModuleInfo moduleInfo,
        IInteractionService interactionService)
    {
        List<HandlerLoadingResult> results = new List<HandlerLoadingResult>();
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

                interactionService.HandleSoftLoadingException(exception);
                results.Add(new HandlerLoadingResult(false, 
                    loadingException: exception));
                continue;
            }
            
            InteractionHandlerInfo info = new InteractionHandlerInfo(
                handlerAttribute.InteractionId, handlerAttribute.RunMode, methodInfo, moduleInfo);
            results.Add(new HandlerLoadingResult(true, info));
        }

        return results;
    }

    private static IEnumerable<TypeInfo> SearchModules(Assembly assembly, 
        IInteractionService interactionService)
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

    private static bool IsValidModuleDefinition(TypeInfo typeInfo)
    {
        return typeInfo is {
            IsPublic: true, IsAbstract: false, IsNested: false,
            ContainsGenericParameters: false,
        };
    }

    private static bool IsValidHandlerDefinition(MethodInfo methodInfo)
    {
        return methodInfo is {
            IsPublic: true, IsStatic: false, IsAbstract: false,
            ContainsGenericParameters: false
        };
    }
}