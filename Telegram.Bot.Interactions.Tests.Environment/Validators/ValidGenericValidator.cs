using Telegram.Bot.Interactions.Attributes.Validators;
using Telegram.Bot.Interactions.Tests.Environment.Responses;
using Telegram.Bot.Interactions.Validators;

namespace Telegram.Bot.Interactions.Tests.Environment.Validators;

/// <summary>
/// Tests valid validator that validates generic response
/// and accept any configurations that is targeted to <see cref="IAbstractResponse"/>.
/// </summary>
[ConfigurableWithAnyOfMyType]
public class ValidGenericValidator : ResponseValidator<IAbstractResponse>
{
    public override ValueTask<bool> ValidateAsync(IAbstractResponse response) { throw new NotImplementedException(); }
}