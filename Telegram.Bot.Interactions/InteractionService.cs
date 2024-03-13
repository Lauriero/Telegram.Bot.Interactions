using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualBasic;

using Telegram.Bot.Interactions.InteractionHandlers;
using Telegram.Bot.Interactions.Model;
using Telegram.Bot.Interactions.Model.Descriptors;
using Telegram.Bot.Interactions.Model.Descriptors.Loading;
using Telegram.Bot.Interactions.Model.Descriptors.Loading.Abstraction;
using Telegram.Bot.Interactions.Utilities.DependencyInjection;
using Telegram.Bot.Interactions.Utilities.Reflection;

namespace Telegram.Bot.Interactions;

/// <inheritdoc />
public class InteractionService : IInteractionService
{
    /// <inheritdoc />
    public ILogger<IInteractionService> Logger { get; }

    public bool StrictLoadingModeEnabled { get; set; } = false;
    
    public IReadOnlyList<IInteraction> LoadedInteractions { get; }
    
    public IReadOnlyDictionary<Type, InteractionModuleInfo> LoadedModules { get; }

    private SemaphoreSlim _lock;
    private IServiceProvider? _serviceProvider;
    private readonly List<IInteraction> _loadedInteractions;
    private readonly Dictionary<Type, InteractionModuleInfo> _loadedModules;
    
    public InteractionService()
    {
        _lock = new SemaphoreSlim(1, 1);
        Logger = new NullLogger<IInteractionService>();

        _loadedInteractions = new List<IInteraction>();
        LoadedInteractions  = new ReadOnlyCollection<IInteraction>(_loadedInteractions);

        _loadedModules = new Dictionary<Type, InteractionModuleInfo>();
        LoadedModules  = new ReadOnlyDictionary<Type, InteractionModuleInfo>(_loadedModules);
    }   

    /// <inheritdoc />
    public async Task<MultipleLoadingResult<ModuleLoadingResult>> 
        LoadInteractionModulesAsync(Assembly interactionsAssembly, 
            IServiceProvider? serviceProvider = null)
    {
        _serviceProvider = serviceProvider ?? new EmptyServiceProvider();
        await _lock.WaitAsync().ConfigureAwait(false);
        try {
            IList<ModuleLoadingResult> loadingResults = 
                DescriptorBuilders.BuildModuleInfos(interactionsAssembly, _serviceProvider,
                this);
            
            foreach (ModuleLoadingResult moduleLoadingResult in loadingResults) {
                if (moduleLoadingResult.Loaded) {
                    _loadedModules.Add(moduleLoadingResult.Info.ModuleType, moduleLoadingResult.Info);
                }
            }
            
            MultipleLoadingResult<ModuleLoadingResult> result = 
                new MultipleLoadingResult<ModuleLoadingResult>(loadingResults);
        } catch (Exception e) {
            throw;
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