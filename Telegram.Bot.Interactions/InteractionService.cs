using System.Diagnostics.CodeAnalysis;

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
    public bool StrictLoadingModeEnabled { get; set; } = false;
    
    /// <inheritdoc />
    public ILogger<IInteractionService> Logger { get; private set; }
    
    /// <inheritdoc />
    public IEntitiesLoader Loader { get; private set; }
    
    /// <inheritdoc />
    public ILoadedEntitiesRegistry Registry { get; private set; }
    
    public InteractionService()
    {
        ApplyDependencies();
    }
    
    public InteractionService(ILogger<IInteractionService> logger, IEntitiesLoader loader, 
        ILoadedEntitiesRegistry registry)
    {
        ApplyDependencies(logger, loader, registry);
    }

    [MemberNotNull(nameof(Logger))]
    [MemberNotNull(nameof(Loader))]
    [MemberNotNull(nameof(Registry))]
    private void ApplyDependencies(ILogger<IInteractionService>? logger = null, 
        IEntitiesLoader? loader = null, ILoadedEntitiesRegistry? registry = null)
    {
        IServiceProvider provider = DefaultServiceProvider.Instance;
        
        Logger   = logger   ?? provider.GetRequiredService<ILogger<InteractionService>>();
        Registry = registry ?? provider.GetRequiredService<ILoadedEntitiesRegistry>();

        Loader   = loader ?? provider.GetRequiredService<IEntitiesLoader>();
        if (Loader is EntitiesLoader defaultLoader) {
            defaultLoader.InteractionService = this;
        }
    }
}