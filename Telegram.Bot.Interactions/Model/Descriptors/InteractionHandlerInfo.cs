using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Telegram.Bot.Interactions.Model.Context;
using Telegram.Bot.Interactions.Model.Descriptors.Config;

namespace Telegram.Bot.Interactions.Model.Descriptors;

public class InteractionHandlerInfo
{
    public readonly uint InteractionId;
    
    /// <inheritdoc cref="HandlerRunMode"/>
    public readonly HandlerRunMode RunMode;

    public string Name => MethodInfo.Name; 
        
    /// <summary>
    /// Reflection info about the handler method.
    /// </summary>
    public readonly MethodInfo MethodInfo;
    
    
    /// <summary>
    /// Descriptor of the module that contains this handler.
    /// </summary>
    public readonly InteractionModuleInfo Module;

    /// <summary>
    /// Specifies whether the handler accepts the <see cref="IInteractionContext{TResponse}"/>
    /// with TResponse set to a specific response type, in which case
    /// <see cref="SpecificContextResponseType"/> will contain the type of the response.
    /// </summary>
    [MemberNotNullWhen(true, nameof(SpecificContextResponseType))]
    public bool AcceptsSpecificContext { get; }
    public Type? SpecificContextResponseType { get; }

    /// <summary>
    /// Specifies whether this handler has a return type of <see cref="Task"/>.
    /// </summary>
    public bool IsAsync { get; }

    /// <summary>
    /// Specifies whether this handler has the second parameter of <see cref="CancellationToken"/>. 
    /// </summary>
    public bool IsCancellable { get; }
    
    public InteractionHandlerInfo(uint interactionId, HandlerRunMode runMode, 
        MethodInfo methodInfo, InteractionModuleInfo module, 
        bool acceptsSpecificContext = false, bool isAsync = false, bool isCancellable = false, 
        Type? specificContextResponseType = null)
    {
        Module                      = module;
        IsAsync                     = isAsync;
        RunMode                     = runMode;
        MethodInfo                  = methodInfo;
        IsCancellable               = isCancellable;
        InteractionId               = interactionId;
        AcceptsSpecificContext      = acceptsSpecificContext;
        SpecificContextResponseType = specificContextResponseType;
    }
}