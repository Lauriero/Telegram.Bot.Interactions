﻿using Telegram.Bot.Interactions.Model.Responses.Abstraction;

namespace Telegram.Bot.Interactions.Builders.InteractionResponses;

/// <summary>
/// Universal interface for response builders.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IResponseModelBuilder<out TResponse>
    where TResponse : class, IUserResponse
{
    /// <summary>
    /// Builds the generic response model.
    /// </summary>
    /// <returns></returns>
    IResponseModel<TResponse> Build();
}