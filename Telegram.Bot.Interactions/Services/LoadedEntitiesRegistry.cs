﻿using System.Collections.Concurrent;
using System.Collections.ObjectModel;

using Telegram.Bot.Interactions.Exceptions.Modules;
using Telegram.Bot.Interactions.Model.Descriptors;
using Telegram.Bot.Interactions.Services.Abstraction;
using Telegram.Bot.Interactions.Utilities.Collections;

namespace Telegram.Bot.Interactions.Services;

public class LoadedEntitiesRegistry : ILoadedEntitiesRegistry
{
    public IReadOnlyDictionary<int, InteractionInfo> Interactions { get; }
    public IReadOnlyDictionary<Type, InteractionModuleInfo> InteractionModules { get; }
    public IReadOnlyDictionary<Type, DefaultEntityCollection<ResponseParserInfo>> 
        ResponseParsers { get; }

    private readonly ConcurrentDictionary<int, InteractionInfo> _interactions;
    private readonly ConcurrentDictionary<Type, InteractionModuleInfo> _interactionModules;
    private readonly ConcurrentDictionary<Type, DefaultEntityCollection<ResponseParserInfo>> _responseParsers;

    private readonly ConcurrentDictionary<Type, List<ResponseParserInfo>> _responseParsersInternal;
    
    public LoadedEntitiesRegistry()
    {
        _interactions       = new ConcurrentDictionary<int, InteractionInfo>();
        _interactionModules = new ConcurrentDictionary<Type, InteractionModuleInfo>();
        _responseParsers    = new ConcurrentDictionary<Type, DefaultEntityCollection<ResponseParserInfo>>();
        _responseParsersInternal    = new ConcurrentDictionary<Type, List<ResponseParserInfo>>();
        
        Interactions       = new ReadOnlyDictionary<int, InteractionInfo>(_interactions);
        InteractionModules = new ReadOnlyDictionary<Type, InteractionModuleInfo>(_interactionModules);
        ResponseParsers    = new ReadOnlyDictionary<Type, DefaultEntityCollection<ResponseParserInfo>>(_responseParsers);
    }

    public void RegisterInteraction(InteractionInfo interactionInfo)
    {
        if (!_interactions.TryAdd(interactionInfo.Interaction.Id, interactionInfo)) {
            throw new EntityRegistrationException<InteractionInfo>(interactionInfo,
                $"Interaction {interactionInfo.Interaction.Id} is already registered");
        }
    }

    public void RegisterInteractionModule(InteractionModuleInfo moduleInfo)
    {
        if (!_interactionModules.TryAdd(moduleInfo.Type, moduleInfo)) {
            throw new EntityRegistrationException<InteractionModuleInfo>(moduleInfo,
                $"Info about the module {moduleInfo.Type} is already presented " +
                "in the registry");
        }
    }

    public void RegisterResponseParser(ResponseParserInfo parserInfo)
    {
        if (!_responseParsers.ContainsKey(parserInfo.TargetResponseType)) {
            List<ResponseParserInfo> internalList = new() { parserInfo };
            _responseParsers.TryAdd(parserInfo.TargetResponseType, 
                new DefaultEntityCollection<ResponseParserInfo>(internalList));
            _responseParsersInternal.TryAdd(parserInfo.TargetResponseType, internalList);
            
            return;
        }

        List<ResponseParserInfo> storedParsers = _responseParsersInternal[parserInfo.TargetResponseType];
        if (storedParsers.Any(p => p.ParserType.IsEquivalentTo(parserInfo.ParserType))) {
            throw new EntityRegistrationException<ResponseParserInfo>(parserInfo,
                $"Attempt to register the parser {parserInfo.ParserType} that is already " +
                $"registered.");
        }
        
        if (parserInfo.Default && storedParsers.Any(i => i.Default)) {
            throw new EntityRegistrationException<ResponseParserInfo>(parserInfo,
                $"Attempt to register the default parser {parserInfo.ParserType} for the " +
                $"response {parserInfo.TargetResponseType} that already have default parser.");
        }

        storedParsers.Add(parserInfo);
    }
}