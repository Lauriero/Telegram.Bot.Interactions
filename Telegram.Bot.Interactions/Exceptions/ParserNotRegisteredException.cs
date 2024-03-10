namespace Telegram.Bot.Interactions.Exceptions;

public class ParserNotRegisteredException<TResponse> : Exception
{
    public ParserNotRegisteredException() 
        : base($"Attempt to build response model of the response type, " +
               $"that does not have a default parser registered, " +
               $"without explicitly setting parser type") { }
}