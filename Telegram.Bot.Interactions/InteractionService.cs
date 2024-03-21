using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Telegram.Bot.Interactions.Services;
using Telegram.Bot.Interactions.Services.Abstraction;
using Telegram.Bot.Interactions.Utilities.DependencyInjection;

namespace Telegram.Bot.Interactions;

/// <inheritdoc />
public class InteractionService : IInteractionService
{
    /// <inheritdoc />
    public IEntitiesLoader Loader { get; private set; }
    
    /// <inheritdoc />
    public ILoadedEntitiesRegistry Registry { get; private set; }

    public IConfigurationService Config { get; private set; }
    
    private ILogger<InteractionService> _logger;
    public InteractionService()
    {
        InternalInit();
    }
    
    [UsedImplicitly]
    public InteractionService(ILogger<InteractionService> logger, IEntitiesLoader loader, 
        ILoadedEntitiesRegistry registry, IConfigurationService config)
    {
        InternalInit(logger, loader, registry, config);
    }

    [MemberNotNull(nameof(Loader))]
    [MemberNotNull(nameof(Registry))]
    [MemberNotNull(nameof(Config))]
    [MemberNotNull(nameof(_logger))]
    private void InternalInit(ILogger<InteractionService>? logger = null,
        IEntitiesLoader? loader = null, ILoadedEntitiesRegistry? registry = null,
        IConfigurationService? config = null)
    {
        IServiceProvider provider = DefaultServiceProvider.BuildDefaultServiceProvider();

        Config   = config   ?? provider.GetRequiredService<IConfigurationService>(); 
        _logger  = logger   ?? provider.GetRequiredService<ILogger<InteractionService>>();
        Registry = registry ?? provider.GetRequiredService<ILoadedEntitiesRegistry>();
        
        Loader = loader ?? provider.GetRequiredService<IEntitiesLoader>();
        if (Loader is EntitiesLoader defaultLoader) {
            defaultLoader.InteractionService = this;
        }

        bool strictTemp = Config.StrictLoadingModeEnabled;
        Config.StrictLoadingModeEnabled = true;
        Loader.LoadResponseParsers(Assembly.GetAssembly(typeof(InteractionService))!);
        Loader.LoadResponseValidators(Assembly.GetAssembly(typeof(InteractionService))!);
        Config.StrictLoadingModeEnabled = strictTemp;
    }
}