using Telegram.Bot.Interactions.Attributes.Validators;
using Telegram.Bot.Interactions.Tests.Environment.Responses;
using Telegram.Bot.Interactions.Validators;
using Telegram.Bot.Interactions.Validators.Configs;

namespace Telegram.Bot.Interactions.Tests.Environment.Validators;

[ConfigurableWith(typeof(IResponseModelConfig<>))]
[ConfigurableWith(typeof(IResponseValidator<>))]
public class InvalidConfigValidator : ResponseValidator<IAbstractResponse>
{
    public override ValueTask<bool> ValidateAsync(IAbstractResponse response) { throw new NotImplementedException(); }
}