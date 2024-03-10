using Telegram.Bot.Interactions.Attributes;
using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;
using Telegram.Bot.Types;

namespace Telegram.Bot.Interactions.Parsers;

[DefaultParser(typeof(TextResponse))]
public class TextResponseParser : IResponseParser<TextResponse>
{
    public bool CanParse(Message message)
    {
        throw new NotImplementedException();
    }

    public ValueTask<TextResponse?> ParseResponseAsync(Message telegramMessage)
    {
        throw new NotImplementedException();
    }
}