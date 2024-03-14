using System.Reflection;

using Telegram.Bot.Interactions.Model.Descriptors;
using Telegram.Bot.Interactions.Model.Descriptors.Loading.Abstraction;
using Telegram.Bot.Interactions.Tests.Environment;

namespace Telegram.Bot.Interactions.Tests.InteractionModules;

[Order(1)]
public class ReflectionTests
{
    private Assembly _environmentAssembly = null!;

    [SetUp]
    public void Setup()
    {
        _environmentAssembly 
            = Assembly.GetAssembly(typeof(TestInteraction)) 
              ?? throw new InvalidOperationException("Test environment assembly " +
                                                     "was not found");
    }

    [Test]
    public async Task TestModulesLoading_NoSP_NoStrict()
    {
        InstanceTests.Service.StrictLoadingModeEnabled = false;
        
        GenericMultipleLoadingResult<InteractionModuleInfo> loadingResult = 
            await InstanceTests.Service.Loader.LoadInteractionModulesAsync(
                _environmentAssembly);
        
        Assert.That(loadingResult.Loaded, Is.False);
        ;
    }
}