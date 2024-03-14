using System.Collections.Concurrent;
using System.Collections.ObjectModel;

using Telegram.Bot.Interactions.Exceptions.Modules;
using Telegram.Bot.Interactions.Model.Descriptors;
using Telegram.Bot.Interactions.Services.Abstraction;

namespace Telegram.Bot.Interactions.Services;

public class LoadedEntitiesRegistry : ILoadedEntitiesRegistry
{
    public IReadOnlyDictionary<int, InteractionInfo> Interactions { get; }
    public IReadOnlyDictionary<Type, InteractionModuleInfo> InteractionModules { get; }
    public IReadOnlyDictionary<Type, IReadOnlyList<ResponseParserInfo>> ResponseParsers { get; }
    public IReadOnlyDictionary<Type, IReadOnlyList<ResponseValidatorInfo>> ResponseValidators { get; }

    private readonly ConcurrentDictionary<int, InteractionInfo> _interactions;
    private readonly ConcurrentDictionary<Type, InteractionModuleInfo> _interactionModules;
    private readonly ConcurrentDictionary<Type, IReadOnlyList<ResponseParserInfo>> _responseParsers;
    private readonly ConcurrentDictionary<Type, IReadOnlyList<ResponseValidatorInfo>> _responseValidators;

    private readonly ConcurrentDictionary<Type, List<ResponseParserInfo>> _responseParsersInternal;
    private readonly ConcurrentDictionary<Type, List<ResponseValidatorInfo>> _responseValidatorsInternal;
    
    public LoadedEntitiesRegistry()
    {
        _interactions       = new ConcurrentDictionary<int, InteractionInfo>();
        _interactionModules = new ConcurrentDictionary<Type, InteractionModuleInfo>();
        _responseParsers    = new ConcurrentDictionary<Type, IReadOnlyList<ResponseParserInfo>>();
        _responseValidators = new ConcurrentDictionary<Type, IReadOnlyList<ResponseValidatorInfo>>();

        _responseParsersInternal    = new ConcurrentDictionary<Type, List<ResponseParserInfo>>();
        _responseValidatorsInternal = new ConcurrentDictionary<Type, List<ResponseValidatorInfo>>();
        
        Interactions       = new ReadOnlyDictionary<int, InteractionInfo>(_interactions);
        InteractionModules = new ReadOnlyDictionary<Type, InteractionModuleInfo>(_interactionModules);
        ResponseParsers    = new ReadOnlyDictionary<Type, IReadOnlyList<ResponseParserInfo>>(_responseParsers);
        ResponseValidators = new ReadOnlyDictionary<Type, IReadOnlyList<ResponseValidatorInfo>>(_responseValidators);
    }

    public void RegisterLoadedInteraction(InteractionInfo interactionInfo)
    {
        if (!_interactions.TryAdd(interactionInfo.Interaction.Id, interactionInfo)) {
            throw new EntityRegistrationException<InteractionInfo>(interactionInfo,
                $"Interaction {interactionInfo.Interaction.Id} is already registered");
        }
    }

    public void RegisterLoadedInteractionModule(InteractionModuleInfo moduleInfo)
    {
        if (!_interactionModules.TryAdd(moduleInfo.ModuleType, moduleInfo)) {
            throw new EntityRegistrationException<InteractionModuleInfo>(moduleInfo,
                $"Info about the module {moduleInfo.ModuleType} is already presented " +
                "in the registry");
        }
    }

    public void RegisterResponseParser(ResponseParserInfo parserInfo)
    {
        if (!_responseParsers.ContainsKey(parserInfo.TargetResponseType)) {
            List<ResponseParserInfo> internalList = new() { parserInfo };
            _responseParsers.TryAdd(parserInfo.TargetResponseType, internalList);
            _responseParsersInternal.TryAdd(parserInfo.TargetResponseType, internalList);
            
            return;
        }

        List<ResponseParserInfo> storedParsers = _responseParsersInternal[parserInfo.TargetResponseType];
        if (parserInfo.Default && storedParsers.Any(i => i.Default)) {
            throw new EntityRegistrationException<ResponseParserInfo>(parserInfo,
                $"Attempt to register the default parser {parserInfo.ParserType} for the " +
                $"response {parserInfo.TargetResponseType} that already have default parser.");
        }

        storedParsers.Add(parserInfo);
    }

    public void RegisterResponseValidator(ResponseValidatorInfo validatorInfo)
    {
        if (!_responseParsers.ContainsKey(validatorInfo.TargetResponseType)) {
            List<ResponseValidatorInfo> internalList = new() { validatorInfo };
            _responseValidators.TryAdd(validatorInfo.TargetResponseType, internalList);
            _responseValidatorsInternal.TryAdd(validatorInfo.TargetResponseType, internalList);
            
            return;
        }

        List<ResponseValidatorInfo> storedValidators = _responseValidatorsInternal[validatorInfo.TargetResponseType];
        if (validatorInfo.Default && storedValidators.Any(i => i.Default)) {
            throw new EntityRegistrationException<ResponseValidatorInfo>(validatorInfo,
                $"Attempt to register the default validator{validatorInfo.ValidatorType} for " +
                $"the response {validatorInfo.TargetResponseType} that already have default validator.");
        }

        storedValidators.Add(validatorInfo);
    }
}