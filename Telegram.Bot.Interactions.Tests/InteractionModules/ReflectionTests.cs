using System.Reflection;

using Telegram.Bot.Interactions.Model.Descriptors.Loading;
using Telegram.Bot.Interactions.Model.Descriptors.Loading.Abstraction;

namespace Telegram.Bot.Interactions.Tests.InteractionModules;

[Order(1)]
public class ReflectionTests
{
    private Assembly _environmentAssembly = null!;

    [SetUp]
    public void Setup()
    {
        _environmentAssembly 
            = Assembly.GetAssembly(typeof(IInteractionService)) 
              ?? throw new InvalidOperationException("Test environment assembly " +
                                                     "was not found");
    }

    [Test]
    public async Task TestLoading_NoSP()
    {
        MultipleLoadingResult<ModuleLoadingResult> loadingResult = 
            await InstanceTests.Service.LoadInteractionModulesAsync(_environmentAssembly);
        
        Assert.That(loadingResult.Loaded, Is.False);
    }
}