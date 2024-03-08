﻿namespace Telegram.Bot.Interactions.Model.Responses.Abstraction;

/// <summary>
/// Configs the interaction response dependent on the factory.
/// </summary>
public interface IInteractionResponseConfig<out TResponse>
    where TResponse : IUserResponse { }
