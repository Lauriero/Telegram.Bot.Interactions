using Telegram.Bot.Interactions.Model.Responses.Abstraction;

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