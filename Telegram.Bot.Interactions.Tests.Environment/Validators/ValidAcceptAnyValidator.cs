using Telegram.Bot.Interactions.Attributes.Validators;
using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;
using Telegram.Bot.Interactions.Validators;

namespace Telegram.Bot.Interactions.Tests.Environment.Validators;

/// <summary>
/// Tests valid validator that accept any config.
/// </summary>
[ConfigurableWithAny]
public class ValidAcceptAnyValidator : ResponseValidator<TextResponse>
{
    public override ValueTask<bool> ValidateAsync(TextResponse response) { throw new NotImplementedException(); }
}