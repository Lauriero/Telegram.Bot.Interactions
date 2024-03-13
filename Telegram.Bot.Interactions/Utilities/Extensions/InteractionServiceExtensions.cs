using Microsoft.Extensions.Logging;

namespace Telegram.Bot.Interactions.Utilities.Extensions;

public static class InteractionServiceExtensions
{
    public static void HandleSoftLoadingException(this IInteractionService service, Exception exception)
    {
        service.Logger.LogWarning(exception.Message);
        if (service.StrictLoadingModeEnabled) {
            throw exception;
        }
    }
}