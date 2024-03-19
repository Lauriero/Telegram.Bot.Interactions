using Telegram.Bot.Interactions.Model.Descriptors;
using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;
using Telegram.Bot.Interactions.Parsers;
using Telegram.Bot.Interactions.Utilities.Collections;

namespace Telegram.Bot.Interactions.Tests;

[Order(0)]
public class InstanceTests
{
    public static IInteractionService Service = null!;

    public static string[] ValidParserNames { get; } = {
        nameof(TextResponseParser),
    };
    
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