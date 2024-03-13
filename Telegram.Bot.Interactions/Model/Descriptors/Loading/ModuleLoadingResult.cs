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
    public bool Loaded { get; }
    
    public Exception? LoadingException { get; }
    
    public InteractionModuleInfo? Info { get; }
    
    public ModuleLoadingResult(bool loaded, InteractionModuleInfo? info = null, 
        Exception? loadingException = null)
    {
        Info             = info;
        Loaded           = loaded;
        LoadingException = loadingException;
    }
}