namespace Telegram.Bot.Interactions.Exceptions.Modules;

public class ParserLoadingException : Exception
{
    public Type ParserType { get; }
    
    public ParserLoadingException(Type parserType, string message)
        : base($"Loading parser {parserType.FullName} failed: {message}")
    {
        ParserType = parserType;
    }
}