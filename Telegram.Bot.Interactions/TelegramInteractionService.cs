using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;

using Telegram.Bot.Interactions.InteractionHandlers;

namespace Telegram.Bot.Interactions;

public class TelegramInteractionService
{
    private SemaphoreSlim _lock;
    private IServiceProvider? _serviceProvider;

    public TelegramInteractionService()
    {
        _lock = new SemaphoreSlim(1, 1);
    }

    /// <summary>
    /// Loads interaction modules - classes that derive from
    /// <see cref="InteractionModuleBase"/> and will be used to handle
    /// registered <see cref="Interaction"/> between bot and the user.
    /// </summary>
    /// <param name="interactionsAssembly">
    /// The assembly instances, interaction modules are located at, that
    /// will be scanned and the located modules with its handlers will be loaded.  
    /// </param>
    /// <param name="serviceProvider">
    /// Service provider that will be used to create new instances of registered
    /// interaction modules, and has to have the modules registered as singletons.
    /// If not provided, empty provider will be used.
    /// </param>
    public async Task LoadInteractionModules(Assembly interactionsAssembly, 
        IServiceProvider? serviceProvider = null)
    {
        _serviceProvider = serviceProvider;
        await _lock.WaitAsync().ConfigureAwait(false);
        try {
            
        } catch (Exception e) {

        } finally {
            _lock.Release();
        }
    }

    public async Task LoadInteractionConfiguration(Assembly configurationAssembly)
    {
        
    }

    [MemberNotNullWhen(true, nameof(_serviceProvider))]
    private bool EnsureLoaded() => 
        _serviceProvider is not null;
}