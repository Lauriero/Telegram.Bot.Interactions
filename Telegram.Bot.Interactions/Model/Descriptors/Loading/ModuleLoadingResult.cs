using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

using Telegram.Bot.Interactions.Model.Descriptors.Loading.Abstraction;

namespace Telegram.Bot.Interactions.Model.Descriptors.Loading;

/// <summary>
/// Contains loading result of the single module.
/// Contains errors if any occurred during the loading process.
/// </summary>
public class ModuleLoadingResult : ILoadingResult
{
    [MemberNotNullWhen(true, nameof(Info))]
    [MemberNotNullWhen(true, nameof(HandlerLoadingResults))]
    [MemberNotNullWhen(true, nameof(InteractionLoadingResults))]
    public bool Loaded { get; }
    
    public Exception? LoadingException { get; }
    
    public InteractionModuleInfo? Info { get; }

    public IReadOnlyList<GenericLoadingResult<InteractionHandlerInfo>>? 
        HandlerLoadingResults { get; }
    
    public IReadOnlyList<GenericLoadingResult<IInteraction>>? 
        InteractionLoadingResults { get; }
    
    private ModuleLoadingResult(bool loaded, InteractionModuleInfo? info = null,
        IList<GenericLoadingResult<InteractionHandlerInfo>>? handlerLoadingResults = null,
        IList<GenericLoadingResult<IInteraction>>? interactionsLoadingResults = null,
        Exception? loadingException = null)
    {
        Info                  = info;
        Loaded                = loaded;
        LoadingException      = loadingException;

        if (interactionsLoadingResults is not null) {
            InteractionLoadingResults = new ReadOnlyCollection<GenericLoadingResult<IInteraction>>(
                interactionsLoadingResults);
        }

        if (handlerLoadingResults is not null) {
            HandlerLoadingResults = new ReadOnlyCollection<GenericLoadingResult<InteractionHandlerInfo>>
                (handlerLoadingResults);
        }
    }

    public static ModuleLoadingResult FromSuccess(InteractionModuleInfo info, 
        IList<GenericLoadingResult<InteractionHandlerInfo>> handlerLoadingResults,
        IList<GenericLoadingResult<IInteraction>> interactionsLoadingResults)
    {
        return new ModuleLoadingResult(true, info, handlerLoadingResults,
            interactionsLoadingResults);
    }

    public static ModuleLoadingResult FromFailure(Exception exception,
        IList<GenericLoadingResult<InteractionHandlerInfo>>? handlerLoadingResults = null,
        IList<GenericLoadingResult<IInteraction>>? interactionsLoadingResults = null)
    {
        return new ModuleLoadingResult(false, loadingException: exception,
            interactionsLoadingResults: interactionsLoadingResults,
            handlerLoadingResults: handlerLoadingResults);
    }
}