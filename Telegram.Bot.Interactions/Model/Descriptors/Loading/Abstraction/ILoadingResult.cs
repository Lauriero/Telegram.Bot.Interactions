using System.Diagnostics.CodeAnalysis;

namespace Telegram.Bot.Interactions.Model.Descriptors.Loading.Abstraction;

/// <summary>
/// Contains the result of the loading process of the entity.
/// </summary>
public interface ILoadingResult
{
    /// <summary>
    /// Specifies whether the entity was loaded successfully.
    /// </summary>
    [MemberNotNullWhen(true, nameof(LoadingException))]
    bool Loaded { get; }
    
    /// <summary>
    /// Contains <see cref="Exception"/> that occurred while loading the entity
    /// and is the reason the entity was not loaded. 
    /// </summary>
    Exception? LoadingException { get; }
}