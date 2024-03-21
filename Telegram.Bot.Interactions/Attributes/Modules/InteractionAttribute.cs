using Telegram.Bot.Interactions.Model.Descriptors.Config;

namespace Telegram.Bot.Interactions.Attributes.Modules;

/// <summary>
/// Interaction attribute to declare interaction
/// on the handler itself.
/// The 
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class InteractionAttribute : Attribute
{
    public ulong InteractionId { get; protected set; }
    public HandlerRunMode RunMode { get; protected set; }

    public InteractionAttribute(ulong interactionId, 
        HandlerRunMode runMode)
    {
        RunMode       = runMode;
        InteractionId = interactionId;
    }
}