namespace Telegram.Bot.Interactions.Tests;

[Order(0)]
public class InstanceTests
{
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    [TestOf(typeof(InteractionService))]
    public void CreateBasicInstance()
    {
        IInteractionService service = new InteractionService();
    } 
}