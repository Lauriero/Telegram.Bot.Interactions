using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Types;

namespace Telegram.Bot.Interactions.Model.Responses.Implementation.Types;

public class TextResponse : IUserResponse
{
    /// <inheritdoc />
    public ResponseType Type => ResponseType.Text;
    
    /// <summary>
    /// Text from the user message - <see cref="Message.Text"/>.
    /// </summary>
    public string Text { get; } = null!;  
    
}
