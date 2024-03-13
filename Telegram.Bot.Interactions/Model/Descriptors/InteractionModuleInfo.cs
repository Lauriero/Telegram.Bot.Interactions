using Telegram.Bot.Interactions.InteractionHandlers;

namespace Telegram.Bot.Interactions.Model.Descriptors;

public class InteractionModuleInfo
{
    public Type ModuleType { get; }
    public IServiceProvider ServiceProvider { get; }
    public IInteractionService InteractionService { get; }
    public IInteractionModule Instance { get; }
    public List<InteractionHandlerInfo> HandlerInfos { get; } = new();
    
    public InteractionModuleInfo(Type moduleType, IInteractionService interactionService, 
        IServiceProvider serviceProvider, IInteractionModule instance)
    {
        ModuleType         = moduleType;
        ServiceProvider    = serviceProvider;
        Instance      = instance;
        InteractionService = interactionService;
    }
}