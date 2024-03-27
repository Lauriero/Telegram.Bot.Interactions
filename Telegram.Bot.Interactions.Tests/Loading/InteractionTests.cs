using Telegram.Bot.Interactions.Model;
using Telegram.Bot.Interactions.Model.Descriptors.Loading.Abstraction;
using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;
using Telegram.Bot.Interactions.Parsers;
using Telegram.Bot.Interactions.Services;
using Telegram.Bot.Interactions.Tests.Environment.Parsers;
using Telegram.Bot.Interactions.Tests.Environment.Responses;
using Telegram.Bot.Interactions.Tests.Environment.Validators;
using Telegram.Bot.Interactions.Tests.Environment.Validators.Configs;
using Telegram.Bot.Interactions.Validators;
using Telegram.Bot.Interactions.Validators.Configs;

namespace Telegram.Bot.Interactions.Tests.Loading;

[TestFixture]
[TestOf(typeof(EntitiesLoader))]
[TestOf(typeof(LoadedEntitiesRegistry))]
public class InteractionTests : BaseLoadingTests
{
    [Test]
    [Order(0)]
    public void TestValidAlwaysLoading_NoStrict()
    {
        GenericLoadingResult<IInteraction> loadingResult = 
            InteractionService.Loader.LoadInteraction(Environment.Interactions.ValidAlwaysInteraction);
            
        Assert.That(loadingResult.Loaded, Is.True);
        Assert.That(Environment.Interactions.ValidAlwaysInteraction.AvailableResponses, Has.Count.EqualTo(1));

        IResponseModel interactionResponse = Environment.Interactions.ValidAlwaysInteraction.AvailableResponses[0];
        Assert.Multiple(() =>
        {
            Assert.That(interactionResponse.Key, Is.EqualTo(Environment.Interactions.V1_TEST1_KEY));
            Assert.That(interactionResponse.ResponseType.IsEquivalentTo(typeof(TextResponse)));
            Assert.That(interactionResponse.Config!.GetType().IsEquivalentTo(typeof(TextResponseModelConfig)));
            Assert.That(interactionResponse.ResponseParserType!.IsEquivalentTo(typeof(TextResponseParser)));
            Assert.That(interactionResponse.ResponseValidatorType!.IsEquivalentTo(typeof(RichTextResponseValidator)));
        });
    }

    [Test]
    [Order(1)]
    public void TestAfterParsersLoading_NoStrict()
    {
        GenericLoadingResult<IInteraction> loadingResult =
            InteractionService.Loader.LoadInteraction(Environment.Interactions.ValidAfterParsersLoadingInteraction);
        Assert.That(loadingResult.Loaded, Is.False);

        InteractionService.Loader.LoadResponseParsers(EnvironmentAssembly);
        
        loadingResult = InteractionService.Loader.LoadInteraction(Environment.Interactions.ValidAfterParsersLoadingInteraction);
        Assert.Multiple(() =>
        {
            Assert.That(loadingResult.Loaded, Is.True);
            Assert.That(loadingResult.Entity!.AvailableResponses[0]
                                     .ResponseParserType!.IsEquivalentTo(typeof(ValidDefaultTestResponseParser)));
        });
    }

    [Test]
    [Order(2)]
    public void TestAfterValidatorsLoading_NoStrict()
    {
        GenericLoadingResult<IInteraction> loadingResult =
            InteractionService.Loader.LoadInteraction(Environment.Interactions.ValidAfterValidatorLoadingInteraction);
        Assert.That(loadingResult.Loaded, Is.False);

        InteractionService.Loader.LoadResponseParsers(EnvironmentAssembly);
        InteractionService.Loader.LoadResponseValidators(EnvironmentAssembly);
        
        loadingResult = InteractionService.Loader.LoadInteraction(Environment.Interactions.ValidAfterValidatorLoadingInteraction);
        Assert.That(loadingResult.Loaded, Is.True);

        IResponseModel firstResponse = loadingResult.Entity!.AvailableResponses[0];
        IResponseModel secondResponse = loadingResult.Entity!.AvailableResponses[1];
        
        Assert.Multiple(() => {
            Assert.That(firstResponse.ResponseValidator, Is.Not.Null);
            Assert.That(firstResponse.ResponseValidatorType!
                                     .IsEquivalentTo(typeof(ValidGenericValidator)));
            Assert.That(ReferenceEquals(firstResponse.Config, 
                firstResponse.GetValidator<TestResponse>()!.Config));
            
            Assert.That(secondResponse.ResponseValidator, Is.Not.Null);
        });
    }
}