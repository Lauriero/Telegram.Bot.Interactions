namespace Telegram.Bot.Interactions.Tests;

[Order(0)]
public class InstanceTests
{
    public static IInteractionService Service = null!;

    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    [TestOf(typeof(InteractionService))]
    public void CreateBasicInstance()
    {
        Service = new InteractionService();
    } 
}