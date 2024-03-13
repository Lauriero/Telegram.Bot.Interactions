namespace Telegram.Bot.Interactions.Model.Responses.Abstraction;

/// <summary>
/// Contains data about user response to an interaction.
/// </summary>
public interface IUserResponse
{
    /// <summary>
    /// Determines the type of this response.
    /// </summary>
    public ResponseType Type { get; }
}