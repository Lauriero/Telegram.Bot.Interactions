using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Types;

namespace Telegram.Bot.Interactions.Parsers;

public interface IResponseParser<TResponse>
    where TResponse : IUserResponse
{
    public bool CanParse(Message message);
    
    public ValueTask<TResponse?> ParseResponseAsync(Message telegramMessage);
}