using Microsoft.Extensions.Logging;

using Telegram.Bot.Interactions.Services.Abstraction;

namespace Telegram.Bot.Interactions;

public interface IInteractionService
{
    /// <summary>
    /// The strict loading mode throw exceptions on loading errors,
    /// the opposite one do not throw any,
    /// yet accumulates errors in the loading results.
    /// </summary>
    public bool StrictLoadingModeEnabled { get; set; }
    
    /// <summary>
    /// Gets logger that is used by the service to log information and errors.
    /// </summary>
    public ILogger<IInteractionService> Logger { get; }
    
    public IEntitiesLoader Loader { get; }
    
    public ILoadedEntitiesRegistry Registry { get; }
}