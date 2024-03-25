using Telegram.Bot.Interactions.Builders;
using Telegram.Bot.Interactions.Builders.InteractionResponses;
using Telegram.Bot.Interactions.Model;
using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;
using Telegram.Bot.Interactions.Tests.Environment.Parsers;
using Telegram.Bot.Interactions.Tests.Environment.Responses;
using Telegram.Bot.Interactions.Tests.Environment.Validators;
using Telegram.Bot.Interactions.Tests.Environment.Validators.Configs;

namespace Telegram.Bot.Interactions.Tests.Environment;

public class Interactions
{
    public const string V1_TEST1_KEY = "v1_test1";
    public const string V2_TEST1_KEY = "v2_test1";
    public const string V3_TEST1_KEY = "v3_test1";
    
    public static readonly IInteraction[] ValidInteractions = {
        // Tests custom response with defined parser 
        InteractionBuilder<TestInteraction>
            .WithId(TestInteraction.V1)
            .WithResponse(BasicResponseModelBuilder<TestResponse>
                .WithKey(V1_TEST1_KEY)
                .WithValidator<ValidGenericValidator>()
                .WithConfig(new AbstractConfigImpl()))
            .WithResponse(BasicResponseModelBuilder<TextResponse>
                .WithKey(V2_TEST1_KEY)
                .WithParser<ValidTextParser>())
            .WithResponse(BasicResponseModelBuilder<TestResponse>
                .WithKey(V3_TEST1_KEY))
            .Build(),
    };
}