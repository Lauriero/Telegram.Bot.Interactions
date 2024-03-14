using System.Collections;
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

using Interaction = Microsoft.VisualBasic.Interaction;

namespace Telegram.Bot.Interactions;

/// <inheritdoc />
public class InteractionService : IInteractionService
{
    /// <inheritdoc />
    public ILogger<IInteractionService> Logger { get; }

    public bool StrictLoadingModeEnabled { get; set; } = false;
    
    public IReadOnlyDictionary<int, InteractionInfo> LoadedInteractions { get; }
    
    public IReadOnlyDictionary<Type, InteractionModuleInfo> LoadedModules { get; }

    private IServiceProvider? _serviceProvider;
    private readonly SemaphoreSlim _lock;
    private readonly Dictionary<int, InteractionInfo> _loadedInteractions;
    private readonly Dictionary<Type, InteractionModuleInfo> _loadedModules;
    
    public InteractionService()
    {
        _lock = new SemaphoreSlim(1, 1);
        Logger = new NullLogger<IInteractionService>();

        _loadedInteractions = new Dictionary<int, InteractionInfo>();
        LoadedInteractions  = new ReadOnlyDictionary<int, InteractionInfo>(_loadedInteractions);

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

            List<IInteraction>           interactionsList    = new List<IInteraction>();
            List<InteractionHandlerInfo> interactionHandlers = new List<InteractionHandlerInfo>();
            foreach (ModuleLoadingResult moduleLoadingResult in loadingResults) {
                if (!moduleLoadingResult.Loaded) {
                    continue;
                }
                
                InteractionModuleInfo moduleInfo = moduleLoadingResult.Info;
                interactionHandlers.AddRange(moduleInfo.HandlerInfos);
                interactionsList.AddRange(moduleInfo.Instance.DeclareInteractions());
                
                _loadedModules.Add(moduleInfo.ModuleType, moduleInfo);
            }

            foreach (IInteraction interaction in interactionsList) {
                bool handlerFound = false;
                foreach (InteractionHandlerInfo handlerInfo in interactionHandlers) {
                    if (interaction.Id == handlerInfo.InteractionId) {
                        handlerFound = true;
                        _loadedInteractions.Add(interaction.Id, new InteractionInfo(interaction, handlerInfo));
                        break;
                    }
                }

                if (!handlerFound) {
                    Logger.LogWarning("Handler for the interaction {id} was not found, " +
                                      "the interaction will not be handled", interaction.Id);
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