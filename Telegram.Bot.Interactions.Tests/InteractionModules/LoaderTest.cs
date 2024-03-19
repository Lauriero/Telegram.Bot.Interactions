using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using Telegram.Bot.Interactions.Exceptions.Modules;
using Telegram.Bot.Interactions.Model.Descriptors;
using Telegram.Bot.Interactions.Model.Descriptors.Config;
using Telegram.Bot.Interactions.Model.Descriptors.Loading;
using Telegram.Bot.Interactions.Model.Descriptors.Loading.Abstraction;
using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;
using Telegram.Bot.Interactions.Parsers;
using Telegram.Bot.Interactions.Tests.Environment;
using Telegram.Bot.Interactions.Tests.Environment.InteractionModules;
using Telegram.Bot.Interactions.Tests.Environment.Parsers;
using Telegram.Bot.Interactions.Tests.Environment.Parsers.Generic;
using Telegram.Bot.Interactions.Tests.Environment.Services;
using Telegram.Bot.Interactions.Utilities.Collections;

namespace Telegram.Bot.Interactions.Tests.InteractionModules;

[Order(1)]
public class LoaderTest
{
    private IInteractionService _interactionService = null!;
    private Assembly _environmentAssembly = null!;

    private static string[] _basicModuleInvalidHandlerNames = {
        nameof(BasicTestInteractionModule.NotDefinedInteractionHandler),
        nameof(BasicTestInteractionModule.InvalidReturnTypeHandler),
        nameof(BasicTestInteractionModule.InvalidArgumentsTypeHandler1),
        nameof(BasicTestInteractionModule.InvalidArgumentsTypeHandler2),
        nameof(BasicTestInteractionModule.InvalidArgumentsTypeHandler3),
        nameof(BasicTestInteractionModule.InvalidArgumentsTypeHandler4),
        nameof(BasicTestInteractionModule.InvalidArgumentsTypeHandler5),
        nameof(BasicTestInteractionModule.InvalidDuplicateHandler),
    };
    
    private static string[] _basicModuleValidHandlerNames = {
        nameof(BasicTestInteractionModule.ValidHandler1),
        nameof(BasicTestInteractionModule.ValidHandler2),
        nameof(BasicTestInteractionModule.ValidHandler3),
    };

    private static readonly string[] _validParserNames = {
        nameof(ValidInheritParser),
        nameof(ValidTextParser),
        nameof(ValidGenericParser),
        nameof(ValidOverrideParser),
    };
    
    private static readonly string[] _invalidParserNames = {
        nameof(InvalidGenericParser),
        nameof(TestParserBase),
    };

    private static readonly string[] _defaultTextParserNames = {
        nameof(TextResponseParser),
    };

    private static readonly string[] _registeredTextParserNames = new[] {
        nameof(ValidTextParser),
        nameof(ValidInheritParser),
        nameof(ValidGenericParser),
        nameof(ValidOverrideParser),
    }.Concat(_defaultTextParserNames).ToArray();

    [SetUp]
    public void Setup()
    {
         _interactionService = new InteractionService();
        _environmentAssembly 
            = Assembly.GetAssembly(typeof(TestInteraction)) 
              ?? throw new InvalidOperationException("Test environment assembly " +
                                                     "was not found");
    }
    
    [Test]
    [Order(0)]
    public void TestDefaultLoading()
    {
        IInteractionService service = new InteractionService();

        IReadOnlyDictionary<Type, DefaultEntityCollection<ResponseParserInfo>> parsers =
            service.Registry.ResponseParsers;
        
        Assert.That(parsers.ContainsKey(typeof(TextResponse)), Is.True);

        IReadOnlyList<ResponseParserInfo> textParsersCollection = parsers[typeof(TextResponse)];
        Assert.That(textParsersCollection, Is.Not.Empty);

        ResponseParserInfo? defaultTextParser = textParsersCollection.FirstOrDefault(p 
            => p.ParserType.IsEquivalentTo(typeof(TextResponseParser)));
        
        Assert.Multiple(() => {
            Assert.That(defaultTextParser, Is.Not.Null);
            Assert.That(defaultTextParser!.Default, Is.True);
            Assert.That(defaultTextParser.TargetResponseType.IsEquivalentTo(typeof(TextResponse)), 
                Is.True);

            Assert.That(defaultTextParser.Instance.GetType().IsEquivalentTo(typeof(TextResponseParser)));
        });
        
        // Test registry
        CollectionAssert.AreEquivalent(_defaultTextParserNames,
            service.Registry.ResponseParsers[typeof(TextResponse)].Select(p => 
                p.ParserType.Name));
    }

    [Test]
    public void TestParsersLoading_NoSP_NoStrict()
    {
        IInteractionService service = new InteractionService();
        service.Config.StrictLoadingModeEnabled = false;
        
        GenericMultipleLoadingResult<ResponseParserInfo> loadingResult = 
            service.Loader.LoadResponseParsers(_environmentAssembly);
        
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
            service.Loader.LoadResponseParser<TextResponse, ValidTextParser>();
        
        Assert.That(singleParserLoadingResult.Loaded, Is.False);
        
        // Test registry
        CollectionAssert.AreEquivalent(_registeredTextParserNames,
            service.Registry.ResponseParsers[typeof(TextResponse)].Select(p => 
                p.ParserType.Name));
    }

    [Test]
    public void TestModulesLoading_NoSP_NoStrict()
    {
        _interactionService.Config.StrictLoadingModeEnabled = false;
        
        MultipleLoadingResult<ModuleLoadingResult> loadingResult = 
            _interactionService.Loader.LoadInteractionModules(
                _environmentAssembly);

        TestLoadingResults(loadingResult);
    }

    [Test]
    public void TestModulesLoading_SP_NoStrict()
    {
        _interactionService.Config.StrictLoadingModeEnabled = false;

        IServiceProvider provider = new ServiceCollection()
            .AddSingleton<ITestService, TestService>()
            .AddSingleton<BasicTestInteractionModule>()
            .BuildServiceProvider();

        provider.GetRequiredService<ITestService>().Test = ITestService.TEST_VARIABLE_VALUE;
        
        MultipleLoadingResult<ModuleLoadingResult> loadingResult = 
            _interactionService.Loader.LoadInteractionModules(
                _environmentAssembly, provider);

        ModuleLoadingResult        basicTestModule = TestLoadingResults(loadingResult);
        BasicTestInteractionModule moduleInstance  = (BasicTestInteractionModule)basicTestModule.Info!.Instance;
        
        Assert.That(moduleInstance.Service!.Test, Is.EqualTo(ITestService.TEST_VARIABLE_VALUE));
        TestLoadingResults(loadingResult);
    }

    [Test]
    public void TestModulesLoading_NoSP_Strict()
    {
        _interactionService.Config.StrictLoadingModeEnabled = true;
        Assert.Throws<HandlerLoadingException>( () => {
            _interactionService.Loader.LoadInteractionModules(_environmentAssembly);
        });
    }

    private static ModuleLoadingResult TestLoadingResults(MultipleLoadingResult<ModuleLoadingResult> loadingResult)
    {
        Assert.Multiple(() =>
        {
            Assert.That(loadingResult.Loaded, Is.True);
            Assert.That(loadingResult.Entities, Is.Not.Null);
            Assert.That(loadingResult.Entities![0].Loaded, Is.True);
            Assert.That(loadingResult.Entities![0].Info, Is.Not.Null);
        });
        
        ModuleLoadingResult basicTestModule = loadingResult.Entities!.First(e 
            => e.Info!.Type.IsEquivalentTo(typeof(BasicTestInteractionModule)));
        
        IEnumerable<string> failedEntityNames = basicTestModule.HandlerLoadingResults!
            .Where(r => !r.Loaded)
            .Select(r => r.EntityName)
            .ToArray();

        IEnumerable<InteractionHandlerInfo> successHandlerInfos = basicTestModule.HandlerLoadingResults!
            .Where(r => r.Loaded)
            .Select(r => r.Entity!).ToArray();
        
        IEnumerable<string> successEntityNames = successHandlerInfos
            .Select(i => i.Name)
            .ToArray();
        
        Assert.That(basicTestModule.HandlerLoadingResults!, Has.Count.EqualTo(11));
        CollectionAssert.AreEquivalent(_basicModuleInvalidHandlerNames, failedEntityNames);
        CollectionAssert.AreEquivalent(_basicModuleValidHandlerNames, successEntityNames);
        
        InteractionHandlerInfo validHandler1 = successHandlerInfos
            .First(i => i.Name == nameof(BasicTestInteractionModule.ValidHandler1));
        InteractionHandlerInfo validHandler2 = successHandlerInfos
            .First(i => i.Name == nameof(BasicTestInteractionModule.ValidHandler2));
        InteractionHandlerInfo validHandler3 = successHandlerInfos
            .First(i => i.Name == nameof(BasicTestInteractionModule.ValidHandler3));
        
        Assert.Multiple(() => 
        {
            Assert.That(validHandler1 is {
                InteractionId: (int)TestInteraction.I1,
                RunMode: HandlerRunMode.RunSync,
                AcceptsSpecificContext: true,
                IsAsync: false,
                IsCancellable: true,
            } && validHandler1.SpecificContextResponseType.IsEquivalentTo(typeof(TextResponse)), 
                Is.True);
            
            Assert.That(validHandler2 is {
                InteractionId         : (int)TestInteraction.I2,
                RunMode               : HandlerRunMode.RunAsync,
                AcceptsSpecificContext: false,
                IsAsync               : false,
                IsCancellable         : true,
            }, Is.True);
            
            Assert.That(validHandler3 is {
                InteractionId         : (int)TestInteraction.I3,
                RunMode               : HandlerRunMode.Default,
                AcceptsSpecificContext: true,
                IsAsync               : true,
                IsCancellable         : false,
            } && validHandler3.SpecificContextResponseType.IsEquivalentTo(typeof(ImageResponse)), 
                Is.True);
        });

        return basicTestModule;
    }
}