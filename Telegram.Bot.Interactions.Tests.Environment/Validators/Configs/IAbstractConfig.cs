using Telegram.Bot.Interactions.Tests.Environment.Responses;
using Telegram.Bot.Interactions.Validators.Configs;

namespace Telegram.Bot.Interactions.Tests.Environment.Validators.Configs;

public interface IAbstractConfig<in TResponse> : IResponseModelConfig<TResponse>
    where TResponse : IAbstractResponse
{
    public string TestParameter { get; }
}