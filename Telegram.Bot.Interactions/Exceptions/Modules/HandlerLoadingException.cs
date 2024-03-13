using System.Reflection;

namespace Telegram.Bot.Interactions.Exceptions.Modules;

/// <summary>
/// Is occurred when the interaction handler was not properly loaded. 
/// </summary>
public class HandlerLoadingException : Exception
{
    public Type ModuleType { get; }
    
    public MethodInfo Handler { get; }

    public HandlerLoadingException(Type moduleType, MethodInfo handler, string message)
        : base($"Loading {moduleType.FullName}.{handler.Name} failed: {message}")
    {
        Handler    = handler;
        ModuleType = moduleType;
    }
}