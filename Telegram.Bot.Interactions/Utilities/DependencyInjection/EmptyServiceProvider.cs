namespace Telegram.Bot.Interactions.Utilities.DependencyInjection;

public class EmptyServiceProvider : IServiceProvider
{
    public object? GetService(Type serviceType)
    {
        return null;
    }
}