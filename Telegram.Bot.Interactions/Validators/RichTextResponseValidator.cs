using Telegram.Bot.Interactions.Attributes.Validators;
using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;
using Telegram.Bot.Interactions.Validators.Configs;

namespace Telegram.Bot.Interactions.Validators;

[ConfigurableWith(typeof(TextResponseModelConfig))]
public class RichTextResponseValidator : ResponseValidator<TextResponse>
{
    public override async ValueTask<bool> ValidateAsync(TextResponse response)
    {
        return true;
    }
}