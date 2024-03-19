using JetBrains.Annotations;

using Telegram.Bot.Interactions.Attributes.Modules;
using Telegram.Bot.Interactions.Builders;
using Telegram.Bot.Interactions.Builders.InteractionResponses;
using Telegram.Bot.Interactions.InteractionHandlers;
using Telegram.Bot.Interactions.Model;
using Telegram.Bot.Interactions.Model.Context;
using Telegram.Bot.Interactions.Model.Descriptors.Config;
using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;
using Telegram.Bot.Interactions.Tests.Environment.Services;

namespace Telegram.Bot.Interactions.Tests.Environment.InteractionModules;

/// <summary>
/// Tests basic reflection utilities against the valid
/// and invalid response handlers.
/// </summary>
public class BasicTestInteractionModule : IInteractionModule
{
    public const string I1_KEY  = "test_text_1";
    public const string I2_KEY  = "test_image_2_1";
    public const string I2_KEY2 = "test_text_2_2";
    public const string I3_KEY  = "test_text_3";

    public ITestService? Service { get; }

    [UsedImplicitly]
    public BasicTestInteractionModule()
    {
        
    }
    
    [UsedImplicitly]
    public BasicTestInteractionModule(ITestService service)
    {
        Service = service;
    }
    
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
                .WithResponse(BasicResponseModelBuilder<TextResponse>
                    .WithKey(I2_KEY2))
                .Build(),
            InteractionBuilder
                .WithId((int)TestInteraction.I3)
                .WithResponse(BasicResponseModelBuilder<ImageResponse>
                    .WithKey(I3_KEY))
                .Build(),
        };
    }

    /// <summary>
    /// Tests valid handler for not declared interaction.
    /// </summary>
    [InteractionHandler((int)TestInteraction.NotDefinedI, HandlerRunMode.RunSync)]
    public void NotDefinedInteractionHandler(IInteractionContext<TextResponse> context,
        CancellationToken token = default)
    {
        
    }
    
    /// <summary>
    /// Tests handler with invalid return type.
    /// </summary>
    [InteractionHandler((int)TestInteraction.I1, HandlerRunMode.RunSync)]
    public string InvalidReturnTypeHandler(IInteractionContext<TextResponse> context,
        CancellationToken token = default)
    {
        return "test";
    }

    /// <summary>
    /// Tests handler with too many parameters.
    /// </summary>
    [InteractionHandler((int)TestInteraction.I1, HandlerRunMode.RunSync)]
    public void InvalidArgumentsTypeHandler1(IInteractionContext<TextResponse> context,
        CancellationToken token = default, string s = "")
    {
        
    }
    
    /// <summary>
    /// Tests handler with invalid second parameter.
    /// </summary>
    [InteractionHandler((int)TestInteraction.I1, HandlerRunMode.RunSync)]
    public void InvalidArgumentsTypeHandler2(IInteractionContext<TextResponse> context,
        string token = "")
    {
        
    }
    
    /// <summary>
    /// Tests handler with invalid first parameter (not context).
    /// </summary>
    [InteractionHandler((int)TestInteraction.I1, HandlerRunMode.RunSync)]
    public void InvalidArgumentsTypeHandler3(List<TextResponse> context,
        CancellationToken token = default)
    {
        
    }
    
    /// <summary>
    /// Tests handler with invalid first parameter (strong type doesn't match with interaction).
    /// </summary>
    [InteractionHandler((int)TestInteraction.I1, HandlerRunMode.RunSync)]
    public void InvalidArgumentsTypeHandler4(IInteractionContext<ImageResponse> context,
        CancellationToken token = default)
    {
        
    }
    
    /// <summary>
    /// Tests handler with invalid first parameter (strong type used on multiple-type response).
    /// </summary>
    [InteractionHandler((int)TestInteraction.I2, HandlerRunMode.RunSync)]
    public void InvalidArgumentsTypeHandler5(IInteractionContext<ImageResponse> context,
        CancellationToken token = default)
    {
        
    }
    
    /// <summary>
    /// Tests valid sync handler (type is strong).
    /// </summary>
    [InteractionHandler((int)TestInteraction.I1, HandlerRunMode.RunSync)]
    public void ValidHandler1(IInteractionContext<TextResponse> context,
        CancellationToken token = default)
    {
        
    }
    
    /// <summary>
    /// Tests valid sync handler, but invalid because it is defined to the
    /// interaction that already has loaded handler. 
    /// </summary>
    [InteractionHandler((int)TestInteraction.I1, HandlerRunMode.RunSync)]
    public void InvalidDuplicateHandler(IInteractionContext<TextResponse> context,
        CancellationToken token = default)
    {
        
    }
    
    /// <summary>
    /// Tests valid sync handler (type is dynamic).
    /// </summary>
    [InteractionHandler((int)TestInteraction.I2, HandlerRunMode.RunAsync)]
    public void ValidHandler2(IInteractionContext<IUserResponse> context,
        CancellationToken token = default)
    {
        
    }
    
    /// <summary>
    /// Tests valid async not cancellable handler (type is strong).
    /// </summary>
    [InteractionHandler((int)TestInteraction.I3)]
    public async Task ValidHandler3(IInteractionContext<ImageResponse> context)
    {
        
    }
}