using Telegram.Bot.Interactions.Services.Abstraction;

namespace Telegram.Bot.Interactions.Model.Descriptors;

internal class InteractionModuleInfo
{
    public Type ModuleType;
    public IServiceProvider ServiceProvider;
    public IInteractionService InteractionService;
    public List<InteractionHandlerInfo> HandlerInfos { get; } = new();

    public InteractionModuleInfo(Type moduleType, IInteractionService interactionService, 
        IServiceProvider serviceProvider)
    {
        ModuleType         = moduleType;
        ServiceProvider    = serviceProvider;
        InteractionService = interactionService;
    }
}