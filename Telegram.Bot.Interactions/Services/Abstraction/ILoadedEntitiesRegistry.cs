using Telegram.Bot.Interactions.Model.Descriptors;

namespace Telegram.Bot.Interactions.Services.Abstraction;

/// <summary>
/// Service that is used to store the loaded entities,
/// takes responsibility of detecting duplicates
/// and throw respective errors when any.
/// </summary>
public interface ILoadedEntitiesRegistry
{
    /// <summary>
    /// Contains the list of loaded interactions,
    /// mapped to the interaction id.
    /// </summary>
    IReadOnlyDictionary<int, InteractionInfo> Interactions { get; }
    
    /// <summary>
    /// Contains the list of loaded interaction modules,
    /// mapped to the interaction module type.
    /// </summary>
    IReadOnlyDictionary<Type, InteractionModuleInfo> InteractionModules { get; }
    
    /// <summary>
    /// Contains the list of loaded parsers for types,
    /// mapped to the response type.
    /// </summary>
    IReadOnlyDictionary<Type, IReadOnlyList<ResponseParserInfo>> ResponseParsers { get; }
    
    /// <summary>
    /// Contains the list of loaded validators for types,
    /// mapped to the response type.
    /// </summary>
    IReadOnlyDictionary<Type, IReadOnlyList<ResponseValidatorInfo>> ResponseValidators { get; }

    void RegisterLoadedInteraction(InteractionInfo interactionInfo);
    
    void RegisterLoadedInteractionModule(InteractionModuleInfo moduleInfo);
    
    void RegisterResponseParser(ResponseParserInfo parserInfo);
    
    void RegisterResponseValidator(ResponseValidatorInfo validatorInfo);
}