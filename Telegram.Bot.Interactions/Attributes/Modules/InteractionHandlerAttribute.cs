using Telegram.Bot.Interactions.Model.Descriptors.Config;

namespace Telegram.Bot.Interactions.Attributes.Modules;

/// <summary>
/// Marks the method as an interaction handler.
/// When user responds to an interaction with the same id provided,
/// this method will be invoked.
/// </summary>
/// <remarks>
/// TODO: Add method parameters restrictions 
/// </remarks>
[AttributeUsage(AttributeTargets.Method)]
public class InteractionHandlerAttribute : Attribute
{
    public uint InteractionId;
    public HandlerRunMode RunMode; 
    
    public InteractionHandlerAttribute(uint interactionId, 
        HandlerRunMode runMode = HandlerRunMode.Default)
    {
        InteractionId = interactionId;
        RunMode  = runMode;
    }
}