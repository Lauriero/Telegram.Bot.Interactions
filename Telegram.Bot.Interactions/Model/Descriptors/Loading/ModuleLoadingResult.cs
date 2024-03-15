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
    public bool Loaded { get; }
    
    public Exception? LoadingException { get; }
    
    public InteractionModuleInfo? Info { get; }

    public IReadOnlyList<GenericLoadingResult<InteractionHandlerInfo>>? 
        HandlerLoadingResults { get; }
    
    private ModuleLoadingResult(bool loaded, InteractionModuleInfo? info = null,
        IList<GenericLoadingResult<InteractionHandlerInfo>>? handlerLoadingResults = null,
        Exception? loadingException = null)
    {
        Info                  = info;
        Loaded                = loaded;
        LoadingException      = loadingException;

        if (handlerLoadingResults is not null) {
            HandlerLoadingResults = new ReadOnlyCollection<GenericLoadingResult<InteractionHandlerInfo>>
                (handlerLoadingResults);
        }
    }

    public static ModuleLoadingResult FromSuccess(InteractionModuleInfo info, 
        IList<GenericLoadingResult<InteractionHandlerInfo>> handlerLoadingResults)
    {
        return new ModuleLoadingResult(true, info, handlerLoadingResults);
    }

    public static ModuleLoadingResult FromFailure(Exception exception)
    {
        return new ModuleLoadingResult(false, loadingException: exception);
    }
}