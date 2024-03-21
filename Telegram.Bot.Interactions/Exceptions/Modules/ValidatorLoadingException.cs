namespace Telegram.Bot.Interactions.Exceptions.Modules;

public class ValidatorLoadingException : Exception
{
    public Type ValidatorType { get; }
    
    public ValidatorLoadingException(Type validatorType, string message)
        : base($"Loading validator {validatorType.FullName} failed: {message}")
    {
        ValidatorType = validatorType;
    }
}