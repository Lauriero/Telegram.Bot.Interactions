using Microsoft.Extensions.Logging;

using Telegram.Bot.Interactions.Services.Abstraction;

namespace Telegram.Bot.Interactions;

public interface IInteractionService
{
    /// <summary>
    /// Gets logger that is used by the service to log information and errors.
    /// </summary>
    public ILogger<IInteractionService> Logger { get; }
    
    public IEntitiesLoader Loader { get; }
    
    public ILoadedEntitiesRegistry Registry { get; } 
    
    public IConfigurationService Config { get; }
}