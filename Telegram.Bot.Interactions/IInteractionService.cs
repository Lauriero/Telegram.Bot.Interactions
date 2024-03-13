using System.Reflection;

using Microsoft.Extensions.Logging;

using Telegram.Bot.Interactions.Model;
using Telegram.Bot.Interactions.InteractionHandlers;
using Telegram.Bot.Interactions.Model.Descriptors;
using Telegram.Bot.Interactions.Model.Descriptors.Loading;
using Telegram.Bot.Interactions.Model.Descriptors.Loading.Abstraction;

namespace Telegram.Bot.Interactions;

public interface IInteractionService
{
    /// <summary>
    /// Gets logger that is used by the service to log information and errors.
    /// </summary>
    public ILogger<IInteractionService> Logger { get; }
    
    /// <summary>
    /// The strict loading mode throw exceptions on loading errors,
    /// the opposite one do not throw any,
    /// yet accumulates errors in the loading results.
    /// </summary>
    public bool StrictLoadingModeEnabled { get; set; }
    
    /// <summary>
    /// Contains the list of loaded interactions.
    /// </summary>
    public IReadOnlyList<IInteraction> LoadedInteractions { get; }
    
    /// <summary>
    /// Contains the list of loaded interaction modules.
    /// </summary>
    public IReadOnlyDictionary<Type, InteractionModuleInfo> LoadedModules { get; }

    /// <summary>
    /// Loads interaction modules - classes that should derive from
    /// <see cref="IInteractionModule"/> and will be used to handle
    /// registered instances of <see cref="IInteraction"/> between
    /// the bot and the user.
    /// </summary>
    /// <param name="interactionsAssembly">
    /// The assembly instances, interaction modules are located at, that
    /// will be scanned and the located modules with its handlers will be loaded.  
    /// </param>
    /// <param name="serviceProvider">
    /// Service provider that will be used to create new instances of registered
    /// interaction modules, and has to have the modules registered.
    /// If not provided, empty provider will be used.
    /// </param>
    /// <remarks>
    /// This method will try resolve interaction modules so be sure to register
    /// any dependencies in provided <see cref="IServiceProvider"/> beforehand.
    /// </remarks>
    public Task<MultipleLoadingResult<ModuleLoadingResult>> LoadInteractionModulesAsync(
        Assembly interactionsAssembly, IServiceProvider? serviceProvider = null);
}