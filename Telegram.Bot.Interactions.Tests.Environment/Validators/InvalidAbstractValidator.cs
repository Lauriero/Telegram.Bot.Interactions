using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;
using Telegram.Bot.Interactions.Validators;

namespace Telegram.Bot.Interactions.Tests.Environment.Validators;

/// <summary>
/// Tests abstract validator definition.
/// </summary>
public abstract class InvalidAbstractValidator : ResponseValidator<TextResponse>
{
    public override ValueTask<bool> ValidateAsync(TextResponse response) { throw new NotImplementedException(); }
}