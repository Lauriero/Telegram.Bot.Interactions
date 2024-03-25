using Telegram.Bot.Interactions.Model;
using Telegram.Bot.Interactions.Model.Descriptors.Loading.Abstraction;
using Telegram.Bot.Interactions.Services;

namespace Telegram.Bot.Interactions.Tests.Loading;

[TestFixture]
[TestOf(typeof(EntitiesLoader))]
[TestOf(typeof(LoadedEntitiesRegistry))]
public class InteractionTests : BaseLoadingTests
{
    [Test]
    public void TestLoading_NoStrict()
    {
        // List<GenericLoadingResult<IInteraction>> validLoadingResults = new();
        // foreach (IInteraction interaction in Environment.Interactions.ValidInteractions) {
        //     GenericLoadingResult<IInteraction> loadingResult = 
        //         InteractionService.Loader.LoadInteraction(interaction);
        //     validLoadingResults.Add(loadingResult);
        //     
        //     Assert.That(loadingResult.Loaded, Is.True);
        // }   
        //
        //
    }
}