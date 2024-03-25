using Telegram.Bot.Interactions.Builders.InteractionResponses;
using Telegram.Bot.Interactions.Model;
using Telegram.Bot.Interactions.Model.Responses.Abstraction;

namespace Telegram.Bot.Interactions.Builders;

public class InteractionBuilder<TInteractionEnumeration> : InteractionBuilder
    where TInteractionEnumeration : Enum
{
    protected InteractionBuilder(uint id) : base(id)
    {
    }

    public static InteractionBuilder WithId(TInteractionEnumeration interaction)
    {
        if (!Enum.GetUnderlyingType(typeof(TInteractionEnumeration)).IsAssignableTo(typeof(UInt32))) {
            throw new ArgumentException("The valid enumeration type should be assignable to UInt32", nameof(interaction));
        }

        return new InteractionBuilder<TInteractionEnumeration>(Convert.ToUInt32(interaction));
    }
}

/// <summary>
/// Is used to build the instances of the <see cref="Interaction"/>.
/// </summary>
public class InteractionBuilder
{
    private uint _id;
    private readonly List<IResponseModel<IUserResponse>> _responses;
    protected InteractionBuilder(uint id)
    {
        _id = id;
        _responses = new List<IResponseModel<IUserResponse>>();
    }

    /// <summary>
    /// Initializes a new building process with the interaction id.
    /// </summary>
    /// <param name="id">Built interaction id.</param>
    public static InteractionBuilder WithId(uint id)
    {
        return new InteractionBuilder(id);
    }

    /// <summary>
    /// Adds the response for the built interaction.
    /// </summary>
    /// <param name="responseModel">Instance that describes the response.</param>
    public InteractionBuilder WithResponse(IResponseModel<IUserResponse> responseModel)
    {
        _responses.Add(responseModel);
        return this;
    }

    /// <summary>
    /// Builds and adds the response for the built interaction.
    /// </summary>
    /// <param name="responseBuilder">Instance of the response builder.</param>
    public InteractionBuilder WithResponse(
        IResponseModelBuilder<IUserResponse> responseBuilder)
    {
        _responses.Add(responseBuilder.Build());
        return this;
    }

    public Interaction Build()
    {
        return new Interaction(_id, _responses);
    }
}