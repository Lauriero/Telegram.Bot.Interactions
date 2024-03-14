namespace Telegram.Bot.Interactions.Services.Abstraction;

public interface IConfigurationService
{
    /// <summary>
    /// The strict loading mode throw exceptions on loading errors,
    /// the opposite one do not throw any,
    /// yet accumulates errors in the loading results.
    /// </summary>
    public bool StrictLoadingModeEnabled { get; set; }
}