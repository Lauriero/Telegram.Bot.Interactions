using Telegram.Bot.Interactions.Model.Descriptors;
using Telegram.Bot.Interactions.Model.Descriptors.Loading.Abstraction;
using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;
using Telegram.Bot.Interactions.Services;
using Telegram.Bot.Interactions.Tests.Environment.Parsers;
using Telegram.Bot.Interactions.Tests.Environment.Parsers.Generic;

namespace Telegram.Bot.Interactions.Tests.Loading;

[TestFixture]
[TestOf(typeof(EntitiesLoader))]
[TestOf(typeof(LoadedEntitiesRegistry))]
public class ParserTests : BaseLoadingTests
{
    private static readonly string[] _validParserNames = {
        nameof(ValidInheritParser),
        nameof(ValidTextParser),
        nameof(ValidGenericParser),
        nameof(ValidOverrideParser),
        nameof(ValidTestResponseParser)
    };
    
    private static readonly string[] _invalidParserNames = {
        nameof(InvalidGenericParser),
        nameof(TestParserBase),
    };
    
    [Test]
    public void TestParsersLoading_NoSP_NoStrict()
    {
        InteractionService.Config.StrictLoadingModeEnabled = false;
        
        GenericMultipleLoadingResult<ResponseParserInfo> loadingResult = 
            InteractionService.Loader.LoadResponseParsers(EnvironmentAssembly);
        
        Assert.That(loadingResult.Loaded, Is.True);
        
        IEnumerable<string> failedParserNames = loadingResult.Entities!
            .Where(e => !e.Loaded)
            .Select(e => e.EntityName);
        
        IEnumerable<string> loadedParserNames = loadingResult.Entities!
             .Where(e => e.Loaded)
             .Select(e => e.EntityName);
        
        CollectionAssert.AreEquivalent(_invalidParserNames, failedParserNames);
        CollectionAssert.AreEquivalent(_validParserNames, loadedParserNames);

        GenericLoadingResult<ResponseParserInfo> singleParserLoadingResult =
            InteractionService.Loader.LoadResponseParser<TextResponse, ValidTextParser>();
        
        Assert.That(singleParserLoadingResult.Loaded, Is.False);
        
        // Test registry
        CollectionAssert.IsSubsetOf(_validParserNames,
            InteractionService.Registry.ResponseParsers[typeof(TextResponse)].Select(p => 
                p.ParserType.Name));
    }
}