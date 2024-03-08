using Telegram.Bot.Interactions.Model;
using Telegram.Bot.Interactions.Model.Responses.Abstraction;

namespace Telegram.Bot.Interactions.Builders;

/// <summary>
/// Is used to build the instances of the <see cref="Interaction"/>.
/// </summary>
public class InteractionBuilder
{
    private int _id;
    private readonly List<IInteractionResponseModel<IUserResponse>> _responses;
    private InteractionBuilder(int id)
    {
        _id = id;
        _responses = new List<IInteractionResponseModel<IUserResponse>>();
    }

    /// <summary>
    /// Initializes a new building process with the interaction id.
    /// </summary>
    /// <param name="id">Built interaction id.</param>
    public static InteractionBuilder WithId(int id)
    {
        return new InteractionBuilder(id);
    }

    /// <summary>
    /// Adds the response for the built interaction.
    /// </summary>
    /// <param name="responseModel">Instance that describes the response.</param>
    public InteractionBuilder WithResponse(IInteractionResponseModel<IUserResponse> responseModel)
    {
        _responses.Add(responseModel);
        return this;
    }

    public Interaction Build()
    {
        return new Interaction(_id, _responses);
    }
}