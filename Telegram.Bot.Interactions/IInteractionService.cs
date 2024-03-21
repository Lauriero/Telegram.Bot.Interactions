using Telegram.Bot.Interactions.Services.Abstraction;

namespace Telegram.Bot.Interactions;

public interface IInteractionService
{
    public IEntitiesLoader Loader { get; }
    
    public ILoadedEntitiesRegistry Registry { get; } 
    
    public IConfigurationService Config { get; }
}