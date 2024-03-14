using Telegram.Bot.Interactions.Attributes.Modules;
using Telegram.Bot.Interactions.Builders;
using Telegram.Bot.Interactions.Builders.InteractionResponses;
using Telegram.Bot.Interactions.InteractionHandlers;
using Telegram.Bot.Interactions.Model;
using Telegram.Bot.Interactions.Model.Context;
using Telegram.Bot.Interactions.Model.Descriptors.Config;
using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;

namespace Telegram.Bot.Interactions.Tests.Environment.InteractionModules;

/// <summary>
/// Tests basic reflection utilities against the valid
/// and invalid response handlers.
/// </summary>
public class BasicInteractionModule : IInteractionModule
{
    public const string I1_KEY = "test_text_1";
    public const string I2_KEY = "test_image_2";
    public const string I3_KEY = "test_text_3";
    
    public IEnumerable<IInteraction> DeclareInteractions()
    {
        return new[] {
            InteractionBuilder
                .WithId((int)TestInteraction.I1)
                .WithResponse(BasicResponseModelBuilder<TextResponse>
                    .WithKey(I1_KEY))
                .Build(),
            InteractionBuilder
                .WithId((int)TestInteraction.I2)
                .WithResponse(BasicResponseModelBuilder<ImageResponse>
                    .WithKey(I2_KEY))
                .Build(),
            InteractionBuilder
                .WithId((int)TestInteraction.I3)
                .WithResponse(BasicResponseModelBuilder<TextResponse>
                    .WithKey(I3_KEY))
                .Build(),
        };
    }

    /// <summary>
    /// Test valid specific context and async method.
    /// </summary>
    [InteractionHandler((int)TestInteraction.I1, HandlerRunMode.RunSync)]
    public async Task TextInteractionHandler1(IInteractionContext<TextResponse> context,
        CancellationToken token = default)
    {
        
    }
    
    /// <summary>
    /// Test generic context and sync method.
    /// </summary>
    [InteractionHandler((int)TestInteraction.I2, HandlerRunMode.RunAsync)]
    public void TextInteractionHandler2(IInteractionContext<IUserResponse> context)
    {
        
    }

    /// <summary>
    /// Test invalid context.
    /// </summary>
    [InteractionHandler((int)TestInteraction.I3)]
    public void TextInteractionHandler3(IInteractionContext<ImageResponse> context,
        CancellationToken token = default)
    {
        
    }
}