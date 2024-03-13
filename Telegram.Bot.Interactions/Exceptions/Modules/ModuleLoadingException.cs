namespace Telegram.Bot.Interactions.Exceptions.Modules;

/// <summary>
/// Is occurred when the interaction module was not properly loaded. 
/// </summary>
public class ModuleLoadingException : Exception
{
    public Type ModuleType { get; }

    public ModuleLoadingException(Type moduleType, string message): base(
        $"Loading {moduleType.FullName} failed: {message}")
    {
        ModuleType = moduleType;
    }
}