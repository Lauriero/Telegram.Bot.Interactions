using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Parsers;

namespace Telegram.Bot.Interactions.Tests.Environment.Parsers.Generic;

public interface IGenericParser<out TParser> : IResponseParser<TParser>
    where TParser : IUserResponse
{
    
}