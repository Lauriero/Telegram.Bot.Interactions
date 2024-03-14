using Telegram.Bot.Interactions.Attributes;
using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;

namespace Telegram.Bot.Interactions.Validators;

/// <summary>
/// Validator that always returns true no matter the config.
/// </summary>
[DefaultValidator(typeof(TextResponse))]
[DefaultValidator(typeof(ImageResponse))]
public class NullResponseValidator : IResponseValidator<IUserResponse>
{
    public ValueTask<bool> ValidateResponseAsync(IUserResponse response, IResponseModelConfig<IUserResponse> config)
    {
        return ValueTask.FromResult(true);
    }
}