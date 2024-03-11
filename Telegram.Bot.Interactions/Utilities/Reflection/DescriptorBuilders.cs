using System.Reflection;

using Microsoft.Extensions.Logging;

using Telegram.Bot.Interactions.Attributes.Modules;
using Telegram.Bot.Interactions.InteractionHandlers;
using Telegram.Bot.Interactions.Model.Descriptors;
using Telegram.Bot.Interactions.Services.Abstraction;

namespace Telegram.Bot.Interactions.Utilities.Reflection;

internal static class DescriptorBuilders
{
    public static TypeInfo ModuleInterfaceType = typeof(IInteractionModule).GetTypeInfo();

    /// <summary>
    /// Scans <see cref="Assembly"/> to search for the interaction modules,
    /// loads them, saving metadata into proper descriptors,
    /// and returns them thereafter.
    /// </summary>
    /// <param name="assembly">
    /// <see cref="Assembly"/> to search for the modules.
    /// </param>
    /// <param name="provider">
    /// <see cref="IServiceProvider"/> that will be used
    /// to create instances of the found modules.
    /// </param>
    /// <param name="interactionService">Requesting service reference</param>
    public static IEnumerable<InteractionModuleInfo> BuildModuleInfos(Assembly assembly, 
        IServiceProvider provider, IInteractionService interactionService)
    {
        List<InteractionModuleInfo> list = new List<InteractionModuleInfo>();
        foreach (TypeInfo typeInfo in SearchModules(assembly, interactionService)) {
            InteractionModuleInfo moduleInfo = new InteractionModuleInfo(
                typeInfo,
                interactionService,
                provider);

            moduleInfo.HandlerInfos.AddRange(
                BuildHandlerInfos(typeInfo, interactionService));
        }
        
        return list;
    }

    public static IEnumerable<InteractionHandlerInfo> BuildHandlerInfos(TypeInfo moduleType,
        IInteractionService interactionService)
    {
        List<InteractionHandlerInfo> methodInfos = new List<InteractionHandlerInfo>();
        foreach (MethodInfo methodInfo in moduleType.GetMethods()) {
            InteractionHandlerAttribute? handlerAttribute =
                methodInfo.GetCustomAttribute<InteractionHandlerAttribute>();

            if (handlerAttribute is null) {
                continue;
            }

            if (!IsValidHandlerDefinition(methodInfo)) {
                interactionService.Logger.LogWarning(
                    "Loading {className}.{methodName}[IID: {interactionId}] failed: "+
                    "handler definition should be a non-static, non-abstract "               +
                    "public method, that doesn't have generic parameters, but found method " +
                    "does not fit in these constraints", moduleType.FullName, 
                    methodInfo.Name, handlerAttribute.InteractionId);
                continue;
            }
            
            InteractionHandlerInfo info = new InteractionHandlerInfo(
                handlerAttribute.InteractionId, handlerAttribute.RunMode, methodInfo);
            methodInfos.Add(info);
        }

        return methodInfos;
    }

    public static IEnumerable<TypeInfo> SearchModules(Assembly assembly, 
        IInteractionService interactionService)
    {
        List<TypeInfo> moduleTypes = new List<TypeInfo>();
        foreach (TypeInfo typeInfo in assembly.DefinedTypes) {
            if (ModuleInterfaceType.IsAssignableFrom(typeInfo)) {
                continue;
            }

            if (!IsValidModuleDefinition(typeInfo)) {
                interactionService.Logger.LogWarning(
                    "Loading {className} failed: module definition should be a non-nested non-abstract "             +
                    "public class, that doesn't have generic parameters, but found class " +
                    "does not fit in these constrained", typeInfo.FullName);
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