using Microsoft.Extensions.Logging;

namespace Telegram.Bot.Interactions.Services.Abstraction;

/// <summary>
/// Main service that is to be used to 
/// </summary>
public interface IInteractionService
{
    internal ILogger<IInteractionService> Logger { get; }
}