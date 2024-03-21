using Telegram.Bot.Interactions.Model.Responses.Abstraction;

namespace Telegram.Bot.Interactions.Tests.Environment.Responses;

public class TestResponse : IAbstractResponse
{
    public ResponseType Type => ResponseType.Other;
}