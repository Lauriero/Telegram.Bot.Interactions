using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;

namespace Telegram.Bot.Interactions.Validators;

public class RichTextResponseValidator : ResponseValidator<TextResponse>
{
    public override async ValueTask<bool> ValidateAsync(TextResponse response)
    {
        return true;
    }
}