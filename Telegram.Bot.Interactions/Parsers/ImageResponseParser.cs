
using MorseCode.ITask;

using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;
using Telegram.Bot.Types;

namespace Telegram.Bot.Interactions.Parsers;

public class ImageResponseParser : IResponseParser<ImageResponse>
{
    public bool CanParse(Message telegramMessage) { throw new NotImplementedException(); }

    public ITask<ImageResponse> ParseResponseAsync(Message telegramMessage) { throw new NotImplementedException(); }
}