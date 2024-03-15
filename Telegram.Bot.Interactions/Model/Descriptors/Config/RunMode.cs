namespace Telegram.Bot.Interactions.Model.Descriptors.Config;

/// <summary>
/// Specifies in what thread the interaction processor will run the handler.
/// </summary>
public enum HandlerRunMode
{
    /// <summary>
    /// Use default value from the config, or <see cref="RunSync"/> if not set.
    /// </summary>
    Default,
    
    /// <summary>
    /// Handler will be invoked on the same thread
    /// the interaction processor is running on.
    /// </summary>
    RunSync,
    
    /// <summary>
    /// The interaction processor will spawn additional thread, disconnecting
    /// from the thread the interaction processor is running on.
    /// </summary>
    /// <remarks>
    /// Any exception that will occur in the handler will not be handled
    /// when using this run mode.
    /// </remarks>
    RunAsync,
}