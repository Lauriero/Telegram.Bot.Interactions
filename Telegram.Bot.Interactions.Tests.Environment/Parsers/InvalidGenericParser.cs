using MorseCode.ITask;

using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Parsers;
using Telegram.Bot.Types;

namespace Telegram.Bot.Interactions.Tests.Environment.Parsers;

/// <summary>
/// Tests invalid parser that does not use implementation
/// of the user response as the type parameter.
/// </summary>
public class InvalidGenericParser : IResponseParser<IUserResponse>
{
    public bool CanParse(Message telegramMessage) { throw new NotImplementedException(); }

    public ITask<IUserResponse> ParseResponseAsync(Message telegramMessage) { throw new NotImplementedException(); }
}