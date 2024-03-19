using Telegram.Bot.Types;

namespace Telegram.Bot.Interactions.Tests.Environment.Parsers;

public class ValidOverrideParser : ValidTextParser
{
    public new bool CanParse(Message telegramMessage)
    {
        throw new NotImplementedException();
    }
}