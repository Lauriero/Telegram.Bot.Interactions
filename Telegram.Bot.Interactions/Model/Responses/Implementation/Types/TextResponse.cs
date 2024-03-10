using Telegram.Bot.Interactions.Model.Responses.Abstraction;

namespace Telegram.Bot.Interactions.Model.Responses.Implementation.Types;

public class TextResponse : IUserResponse
{
    /// <summary>
    /// Text from the user message - <see cref="Message.Text"/>.
    /// </summary>
    public string Text { get; } = null!;  
}
