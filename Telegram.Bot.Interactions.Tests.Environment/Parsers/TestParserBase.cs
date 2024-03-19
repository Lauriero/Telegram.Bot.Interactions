using MorseCode.ITask;

using Telegram.Bot.Interactions.Model.Responses.Implementation.Types;
using Telegram.Bot.Interactions.Parsers;
using Telegram.Bot.Types;

namespace Telegram.Bot.Interactions.Tests.Environment.Parsers;

public abstract class TestParserBase : IResponseParser<TextResponse>
{
    public abstract string Test { get; } 
    
    public bool CanParse(Message telegramMessage) { throw new NotImplementedException(); }

    public ITask<TextResponse> ParseResponseAsync(Message telegramMessage) { throw new NotImplementedException(); }
}