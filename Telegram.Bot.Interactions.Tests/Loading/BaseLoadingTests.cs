using System.Reflection;

using Telegram.Bot.Interactions.Tests.Environment;

namespace Telegram.Bot.Interactions.Tests.Loading;

[Ignore("This is the abstract class for all the loading tests")]
public abstract class BaseLoadingTests
{
    protected Assembly EnvironmentAssembly = null!;
    protected IInteractionService InteractionService = null!;
    
    [SetUp]
    public void Setup()
    {
        InteractionService = new InteractionService();
        EnvironmentAssembly 
            = Assembly.GetAssembly(typeof(TestInteraction)) 
              ?? throw new InvalidOperationException("Test environment assembly was not found");
    }
}