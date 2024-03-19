using System.Collections;
using System.Net.Mime;
using System.Reflection;

using Telegram.Bot.Interactions.Model.Descriptors;
using Telegram.Bot.Interactions.Model.Descriptors.Config;
using Telegram.Bot.Interactions.Model.Descriptors.Loading;
using Telegram.Bot.Interactions.Model.Descriptors.Loading.Abstraction;
using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;
using Telegram.Bot.Interactions.Tests.Environment;
using Telegram.Bot.Interactions.Tests.Environment.InteractionModules;

namespace Telegram.Bot.Interactions.Tests.InteractionModules;

[Order(1)]
public class LoaderTest
{
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

    [SetUp]
    public void Setup()
    {
        _environmentAssembly 
            = Assembly.GetAssembly(typeof(TestInteraction)) 
              ?? throw new InvalidOperationException("Test environment assembly " +
                                                     "was not found");
    }

    [Test]
    public void TestParsersLoading_NoSP_NoStrict()
    {
        
    }

    [Test]
    public async Task TestModulesLoading_NoSP_NoStrict()
    {
        InstanceTests.Service.Config.StrictLoadingModeEnabled = false;
        
        MultipleLoadingResult<ModuleLoadingResult> loadingResult = 
            InstanceTests.Service.Loader.LoadInteractionModules(
                _environmentAssembly);
        
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
    }
}