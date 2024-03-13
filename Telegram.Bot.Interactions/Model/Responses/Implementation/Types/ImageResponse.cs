using Telegram.Bot.Interactions.Model.Responses.Abstraction;

namespace Telegram.Bot.Interactions.Model.Responses.Implementation.Types;

public class ImageResponse : IUserResponse
{
    public ResponseType Type => ResponseType.Image;
}