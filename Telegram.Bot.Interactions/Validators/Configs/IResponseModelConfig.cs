using Telegram.Bot.Interactions.Model.Responses.Abstraction;

namespace Telegram.Bot.Interactions.Validators.Configs;

/// <summary>
/// Configs the interaction response dependent on the factory.
/// </summary>
public interface IResponseModelConfig<in TResponse>
    where TResponse : IUserResponse { }
