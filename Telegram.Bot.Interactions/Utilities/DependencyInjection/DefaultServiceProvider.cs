using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using Telegram.Bot.Interactions.Services;
using Telegram.Bot.Interactions.Services.Abstraction;

namespace Telegram.Bot.Interactions.Utilities.DependencyInjection;

public static class DefaultServiceProvider
{
    private static IServiceProvider? _instance;

    public static IServiceProvider Instance => 
        _instance ??= BuildDefaultServiceProvider();

    private static IServiceProvider BuildDefaultServiceProvider()
    {
        ServiceCollection collection = new ServiceCollection();
        collection.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
        collection.AddSingleton<IEntitiesLoader, EntitiesLoader>(); 
        collection.AddSingleton<IConfigurationService, ConfigurationService>();
        collection.AddSingleton<ILoadedEntitiesRegistry, LoadedEntitiesRegistry>();

        return collection.BuildServiceProvider();
    }
}