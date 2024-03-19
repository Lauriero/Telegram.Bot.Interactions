using MorseCode.ITask;

using Telegram.Bot.Interactions.Attributes;
using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Interactions.Parsers;

[DefaultParser]
public class TextResponseParser : IResponseParser<TextResponse>
{
    public bool CanParse(Message telegramMessage)
    {
        return telegramMessage.Type == MessageType.Text;
    }

    public async ITask<TextResponse> ParseResponseAsync(Message telegramMessage)
    {
        return new TextResponse();
    }
}