using Telegram.Bot.Interactions.InteractionHandlers;

namespace Telegram.Bot.Interactions.Model.Descriptors;

public class InteractionModuleInfo
{
    public Type Type { get; }
    public IServiceProvider ServiceProvider { get; }
    public IInteractionService InteractionService { get; }
    public IInteractionModule Instance { get; }
    public List<InteractionHandlerInfo> HandlerInfos { get; } = new();
    
    public InteractionModuleInfo(Type type, IInteractionService interactionService, 
        IServiceProvider serviceProvider, IInteractionModule instance)
    {
        Type         = type;
        ServiceProvider    = serviceProvider;
        Instance      = instance;
        InteractionService = interactionService;
    }
}