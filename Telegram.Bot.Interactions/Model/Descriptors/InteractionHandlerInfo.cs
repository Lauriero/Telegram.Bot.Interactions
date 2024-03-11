using System.Reflection;

using Telegram.Bot.Interactions.Model.Descriptors.Config;

namespace Telegram.Bot.Interactions.Model.Descriptors;

public class InteractionHandlerInfo
{
    public readonly int InteractionId;
    public readonly HandlerRunMode RunMode;
    public readonly MethodInfo MethodInfo;
    
    public InteractionHandlerInfo(int interactionId, HandlerRunMode runMode, MethodInfo methodInfo)
    {
        InteractionId   = interactionId;
        RunMode         = runMode;
        MethodInfo = methodInfo;
    }
}