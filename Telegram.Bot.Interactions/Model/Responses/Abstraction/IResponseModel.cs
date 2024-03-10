﻿using System.Diagnostics.CodeAnalysis;

namespace Telegram.Bot.Interactions.Model.Responses.Abstraction;

/// <summary>
/// Describes the possible response for a specific interaction.
/// If the response is received, contains the data of the response.
/// </summary>
/// <typeparam name="TResponse">
/// The response type data this response model represents.
/// </typeparam>
public interface IResponseModel<out TResponse>
    where TResponse : IUserResponse
{
    /// <summary>
    /// Is used to identify the response for the interaction.
    /// </summary>
    public string Key { get; }
    
    /// <summary>
    /// Determines whether the user responded to an interaction
    /// according to this response model.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Valid))]
    [MemberNotNullWhen(true, nameof(Response))]
    public bool Responded { get; }
    
    /// <summary>
    /// If set, determines whether that the user response was valid.
    /// Not set if the user hasn't responded.
    /// </summary>
    public bool? Valid { get; }

    /// <summary>
    /// Set if the user has responded to an interaction
    /// and contains data about his response in this case.
    /// </summary>
    public TResponse? Response { get; }
    
    /// <summary>
    /// TODO: Add summary
    /// </summary>
    public Type ResponseParserType { get; }
}