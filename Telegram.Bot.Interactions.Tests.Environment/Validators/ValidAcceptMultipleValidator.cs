using Telegram.Bot.Interactions.Attributes.Validators;
using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;
using Telegram.Bot.Interactions.Tests.Environment.Validators.Configs;
using Telegram.Bot.Interactions.Validators;

namespace Telegram.Bot.Interactions.Tests.Environment.Validators;

/// <summary>
/// Test valid text response validator that accept many configs.
/// </summary>
[ConfigurableWith(typeof(TestTextConfig))]
[ConfigurableWith(typeof(ImageTestConfig))]
[ConfigurableWith(typeof(AbstractConfigImpl))]
public class ValidAcceptMultipleValidator : ResponseValidator<ImageResponse>
{
    public override ValueTask<bool> ValidateAsync(ImageResponse response) { throw new NotImplementedException(); }
}