using System.Diagnostics.CodeAnalysis;

namespace Telegram.Bot.Interactions.Model.Descriptors.Loading;

/// <summary>
/// Contains loading result of the single handler.
/// Contains errors if any occurred during the loading process.
/// </summary>
public class HandlerLoadingResult
{
    [MemberNotNullWhen(true, nameof(Info))]
    public bool Loaded { get; }
    
    public Exception? LoadingException { get; }
    
    public InteractionHandlerInfo? Info { get; }
    
    public HandlerLoadingResult(bool loaded, InteractionHandlerInfo? info = null, 
        Exception? loadingException = null)
    {
        Info             = info;
        Loaded           = loaded;
        LoadingException = loadingException;
    }
}