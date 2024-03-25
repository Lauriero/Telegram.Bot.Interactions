using System.Collections.ObjectModel;

using Telegram.Bot.Interactions.Model.Responses.Abstraction;

namespace Telegram.Bot.Interactions.Model;

/// <inheritdoc />
public class Interaction : IInteraction
{
    /// <inheritdoc />
    public uint Id { get; }

    /// <inheritdoc />
    public IReadOnlyList<IResponseModel> AvailableResponses { get; }

    public Interaction(uint id, IList<IResponseModel> availableResponses)
    {
        Id = id;
        AvailableResponses = 
            new ReadOnlyCollection<IResponseModel>(availableResponses);
    }
}