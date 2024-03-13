using System.Collections.ObjectModel;

using Telegram.Bot.Interactions.Model.Data;
using Telegram.Bot.Interactions.Model.Responses.Abstraction;

namespace Telegram.Bot.Interactions.Model;

/// <inheritdoc />
public class Interaction : IInteraction
{
    /// <inheritdoc />
    public int Id { get; }

    /// <inheritdoc />
    public IReadOnlyList<IResponseModel<IUserResponse>> AvailableResponses { get; }

    public Interaction(int id, IList<IResponseModel<IUserResponse>> availableResponses)
    {
        Id = id;
        AvailableResponses = 
            new ReadOnlyCollection<IResponseModel<IUserResponse>>(availableResponses);
    }
}