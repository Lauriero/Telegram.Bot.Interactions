using Telegram.Bot.Interactions.Builders;
using Telegram.Bot.Interactions.Builders.InteractionResponses;
using Telegram.Bot.Interactions.Model;
using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;
using Telegram.Bot.Interactions.Tests.Environment.Parsers;
using Telegram.Bot.Interactions.Tests.Environment.Responses;
using Telegram.Bot.Interactions.Tests.Environment.Validators;
using Telegram.Bot.Interactions.Tests.Environment.Validators.Configs;
using Telegram.Bot.Interactions.Validators;
using Telegram.Bot.Interactions.Validators.Configs;

namespace Telegram.Bot.Interactions.Tests.Environment;

public class Interactions
{
    public const string V1_TEST1_KEY = "v1_test1";
    public const string V2_TEST1_KEY = "v2_test1";
    public const string V3_TEST1_KEY = "v3_test1";
    public const string V3_TEST2_KEY = "v3_test2";
    
    public const string V3_TEST1_CONFIG_PARAMETER = "v1_test1_cp";
    
    public static readonly IInteraction ValidAlwaysInteraction =
        InteractionBuilder
            .WithId((uint)TestInteraction.V1)
            .WithResponse(BasicResponseModelBuilder<TextResponse>
                .WithKey(V1_TEST1_KEY)
                .WithValidator<RichTextResponseValidator>()
                .WithConfig(new TextResponseModelConfig()))
            .Build();

    // Tests the setting of default parser that will not be loaded automatically
    // Parser should be a ValidDefaultTestResponseParser
    public static readonly IInteraction ValidAfterParsersLoadingInteraction =
        InteractionBuilder
            .WithId((uint)TestInteraction.V2)
            .WithResponse(BasicResponseModelBuilder<TestResponse>
                  .WithKey(V2_TEST1_KEY))
            .Build();
    
    // Tests the interaction with configs and validators, defining them by instance or type,
    // that will not be loaded automatically. The parsers should also be loaded previously.
    public static readonly IInteraction ValidAfterValidatorLoadingInteraction =
        InteractionBuilder
            .WithId((uint)TestInteraction.V3)
            .WithResponse(BasicResponseModelBuilder<TestResponse>
                .WithKey(V3_TEST1_KEY)
                .WithValidator<ValidGenericValidator>()
                .WithConfig(new AbstractConfigImpl(V3_TEST1_CONFIG_PARAMETER)))
            .WithResponse(BasicResponseModelBuilder<TextResponse>
                .WithKey(V3_TEST2_KEY)
                .WithValidator(new ValidAcceptAnyValidator()))
            .Build();
}