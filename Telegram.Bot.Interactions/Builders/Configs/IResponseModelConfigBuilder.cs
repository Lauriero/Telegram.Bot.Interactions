using Telegram.Bot.Interactions.Model.Responses.Abstraction;

namespace Telegram.Bot.Interactions.Builders.Configs;

public interface IResponseModelConfigBuilder<out TResponse>
    where TResponse : IUserResponse
{
    IResponseModelConfig<TResponse> Build();
}