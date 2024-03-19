using Telegram.Bot.Interactions.Attributes;
using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;

namespace Telegram.Bot.Interactions.Validators;

/// <summary>
/// Validator that always returns true no matter the config.
/// </summary>
public class NullResponseValidator : ResponseValidator<IUserResponse>
{
    public override ValueTask<bool> ValidateAsync(IUserResponse response)
    {
        return ValueTask.FromResult(true);
    }
}