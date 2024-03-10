using Telegram.Bot.Interactions.Model.Responses.Abstraction;

namespace Telegram.Bot.Interactions.Validators;

public interface IResponseValidator<in TResponse>
    where TResponse : IUserResponse
{
    public ValueTask<bool> ValidateResponseAsync(TResponse response, IResponseModelConfig<TResponse> config);
}