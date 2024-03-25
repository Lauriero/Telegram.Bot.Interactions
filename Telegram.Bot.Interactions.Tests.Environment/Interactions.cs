using Telegram.Bot.Interactions.Builders;
using Telegram.Bot.Interactions.Builders.InteractionResponses;
using Telegram.Bot.Interactions.Model;
using Telegram.Bot.Interactions.Tests.Environment.Parsers;
using Telegram.Bot.Interactions.Tests.Environment.Responses;

namespace Telegram.Bot.Interactions.Tests.Environment;

public class Interactions
{
    public const string V1_TEST1_KEY = "v1_test1";
    public const string V2_TEST1_KEY = "v2_test1";
    
    
    public static readonly IInteraction[] ValidInteractions = {
        // Tests custom response with defined parser 
        InteractionBuilder<TestInteraction>
            .WithId(TestInteraction.V1)
            .WithResponse(BasicResponseModelBuilder<TestResponse>
                .WithKey(V1_TEST1_KEY)
                .WithParser<ValidTestResponseParser>())
            .Build(),
        
        // Tests custom response with undefined parser 
        InteractionBuilder<TestInteraction>
            .WithId(TestInteraction.V2)
            .WithResponse(BasicResponseModelBuilder<TestResponse>
                .WithKey(V2_TEST1_KEY))
            .Build(),
    };
}