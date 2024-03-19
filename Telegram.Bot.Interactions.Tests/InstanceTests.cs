using Telegram.Bot.Interactions.Model.Descriptors;
using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;
using Telegram.Bot.Interactions.Parsers;
using Telegram.Bot.Interactions.Services.Abstraction;
using Telegram.Bot.Interactions.Utilities.Collections;

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

    [Test]
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
    }
}