using MorseCode.ITask;

using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Types;

namespace Telegram.Bot.Interactions.Parsers;

public interface IResponseParser<out TResponse>
    where TResponse : IUserResponse
{
    public bool CanParse(Message telegramMessage);
    
    public ITask<TResponse> ParseResponseAsync(Message telegramMessage);
}