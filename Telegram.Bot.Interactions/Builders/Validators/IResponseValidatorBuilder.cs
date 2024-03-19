using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Validators;

namespace Telegram.Bot.Interactions.Builders.Validators;

public interface IResponseValidatorBuilder<out TResponse>
    where TResponse : IUserResponse
{
    IResponseValidator<TResponse> Build();
}