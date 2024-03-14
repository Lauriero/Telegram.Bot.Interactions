using System.Reflection;

using Telegram.Bot.Interactions.Model.Descriptors.Config;

namespace Telegram.Bot.Interactions.Model.Descriptors;

public class InteractionHandlerInfo
{
    public readonly int InteractionId;
    public readonly HandlerRunMode RunMode;
    public readonly MethodInfo MethodInfo;
    public readonly InteractionModuleInfo Module;
    
    public InteractionHandlerInfo(int interactionId, HandlerRunMode runMode, MethodInfo methodInfo, InteractionModuleInfo module)
    {
        RunMode       = runMode;
        MethodInfo    = methodInfo;
        Module   = module;
        InteractionId = interactionId;
    }
}