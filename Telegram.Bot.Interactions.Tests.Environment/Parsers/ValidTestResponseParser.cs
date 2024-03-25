using MorseCode.ITask;

using Telegram.Bot.Interactions.Parsers;
using Telegram.Bot.Interactions.Tests.Environment.Responses;
using Telegram.Bot.Types;

namespace Telegram.Bot.Interactions.Tests.Environment.Parsers;

public class ValidTestResponseParser : IResponseParser<TestResponse>
{
    public bool CanParse(Message telegramMessage) { throw new NotImplementedException(); }

    public ITask<TestResponse> ParseResponseAsync(Message telegramMessage) { throw new NotImplementedException(); }
}