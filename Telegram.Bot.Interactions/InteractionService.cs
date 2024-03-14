using System.Diagnostics.CodeAnalysis;

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
    public ILogger<IInteractionService> Logger { get; private set; }
    
    /// <inheritdoc />
    public IEntitiesLoader Loader { get; private set; }
    
    /// <inheritdoc />
    public ILoadedEntitiesRegistry Registry { get; private set; }

    public IConfigurationService Config { get; private set; }

    public InteractionService()
    {
        ApplyDependencies();
    }
    
    [UsedImplicitly]
    public InteractionService(ILogger<IInteractionService> logger, IEntitiesLoader loader, 
        ILoadedEntitiesRegistry registry, IConfigurationService config)
    {
        ApplyDependencies(logger, loader, registry, config);
    }

    [MemberNotNull(nameof(Logger))]
    [MemberNotNull(nameof(Loader))]
    [MemberNotNull(nameof(Registry))]
    [MemberNotNull(nameof(Config))]
    private void ApplyDependencies(ILogger<IInteractionService>? logger = null,
        IEntitiesLoader? loader = null, ILoadedEntitiesRegistry? registry = null,
        IConfigurationService? config = null)
    {
        IServiceProvider provider = DefaultServiceProvider.Instance;

        Logger   = logger   ?? provider.GetRequiredService<ILogger<InteractionService>>();
        Registry = registry ?? provider.GetRequiredService<ILoadedEntitiesRegistry>();
        Config   = config   ?? provider.GetRequiredService<IConfigurationService>(); 
        
        Loader = loader ?? provider.GetRequiredService<IEntitiesLoader>();
        if (Loader is EntitiesLoader defaultLoader) {
            defaultLoader.InteractionService = this;
        }
    }
}