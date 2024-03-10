using Telegram.Bot.Interactions.Model.Responses.Abstraction;

namespace Telegram.Bot.Interactions.Validators;

/// <summary>
/// Validator that always returns true no matter the config.
/// </summary>
public class NullResponseValidator : IResponseValidator<IUserResponse>
{
    public ValueTask<bool> ValidateResponseAsync(IUserResponse response, IResponseModelConfig<IUserResponse> config)
    {
        return ValueTask.FromResult(true);
    }
}